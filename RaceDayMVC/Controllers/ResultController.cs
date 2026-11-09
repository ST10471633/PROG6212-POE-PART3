
using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Models;
using System.Net.Http.Json;

namespace RaceDayMVC.Controllers
{
    public class ResultController : Controller
    {
        private readonly IHttpClientFactory _factory;

        public ResultController(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        private HttpClient GetClient()
        {
            var client = _factory.CreateClient("RaceDayAPI");

            if (Request.Headers.ContainsKey("Cookie"))
                client.DefaultRequestHeaders.TryAddWithoutValidation(
                    "Cookie", Request.Headers.Cookie.ToString());

            return client;
        }

        public async Task<IActionResult> Index()
        {
            var response = await GetClient().GetAsync("api/result");

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode);

            var results = await response.Content
                .ReadFromJsonAsync<List<Result>>();

            return View(results ?? new List<Result>());
        }

        public async Task<IActionResult> MyResults()
        {
            var response = await GetClient().GetAsync("api/result/my-results");

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode);

            var results = await response.Content
                .ReadFromJsonAsync<List<Result>>();

            return View(results ?? new List<Result>());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Result model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await GetClient()
                .PostAsJsonAsync("api/result", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Could not save the result.");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
