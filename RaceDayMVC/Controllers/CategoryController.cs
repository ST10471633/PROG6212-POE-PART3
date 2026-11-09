
using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Models;
using System.Net.Http.Json;

namespace RaceDayMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IHttpClientFactory _factory;

        public CategoryController(IHttpClientFactory factory)
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
            var categories = await GetClient()
                .GetFromJsonAsync<List<Category>>("api/categories");

            return View(categories ?? new List<Category>());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await GetClient()
                .PostAsJsonAsync("api/categories", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Could not create the category.");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await GetClient().GetAsync($"api/categories/{id}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var model = await response.Content
                .ReadFromJsonAsync<Category>();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category model)
        {
            if (id != model.CategoryID)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            var response = await GetClient()
                .PutAsJsonAsync($"api/categories/{id}", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Could not update the category.");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await GetClient()
                .DeleteAsync($"api/categories/{id}");

            if (!response.IsSuccessStatusCode)
                return BadRequest("Could not delete the category.");

            return RedirectToAction(nameof(Index));
        }
    }
}
