
using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace RaceDayMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginView model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var response = await client.PostAsJsonAsync(
                "api/auth/login", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty,
                    "Invalid email or password.");
                return View(model);
            }

            var result = await response.Content.ReadFromJsonAsync<JsonElement>();

            // Forward the API authentication cookie to the browser.
            if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
            {
                foreach (var cookie in cookies)
                {
                    Response.Headers.Append("Set-Cookie", cookie);
                }
            }

            HttpContext.Session.SetInt32(
                "UserID", result.GetProperty("userID").GetInt32());

            HttpContext.Session.SetString(
                "UserName", result.GetProperty("name").GetString() ?? "");

            HttpContext.Session.SetString(
                "UserRole", result.GetProperty("role").GetString() ?? "");

            return RedirectToAction("Index", "Home");
        }

        // GET: Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Role != "Organiser" &&
                model.Role != "Participant")
            {
                ModelState.AddModelError(
                    nameof(model.Role), "Please select a valid role.");

                return View(model);
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var response = await client.PostAsJsonAsync(
                "api/auth/register",
                new
                {
                    model.Name,
                    model.Surname,
                    model.Email,
                    model.Password,
                    model.Role
                });

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty,
                    "Registration failed. The email may already be registered.");

                return View(model);
            }

            return RedirectToAction("Login");
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            using var request = new HttpRequestMessage(
                HttpMethod.Post, "api/auth/logout");

            if (Request.Headers.TryGetValue("Cookie", out var cookie))
            {
                request.Headers.TryAddWithoutValidation(
                    "Cookie", cookie.ToString());
            }

            var response = await client.SendAsync(request);

            HttpContext.Session.Clear();
            Response.Cookies.Delete("RaceDayAuth");

            if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
            {
                foreach (var item in cookies)
                {
                    if (item.Contains("expires=", StringComparison.OrdinalIgnoreCase) ||
                        item.Contains("max-age=0", StringComparison.OrdinalIgnoreCase))
                    {
                        Response.Headers.Append("Set-Cookie", item);
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
