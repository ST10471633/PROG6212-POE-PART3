
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace RaceDayMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpClientFactory _factory;

        public UserController(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _factory.CreateClient("RaceDayAPI");

            if (Request.Headers.ContainsKey("Cookie"))
                client.DefaultRequestHeaders.TryAddWithoutValidation(
                    "Cookie", Request.Headers.Cookie.ToString());

            var response = await client.GetAsync("api/auth/me");

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Login", "Account");

            var profile = await response.Content.ReadFromJsonAsync<ProfileResponse>();

            return View(profile);
        }

        public class ProfileResponse
        {
            public string? Email { get; set; }
            public string? Role { get; set; }
        }
    }
}
