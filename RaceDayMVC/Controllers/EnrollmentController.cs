
using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Models;
using System.Net.Http.Json;

namespace RaceDayMVC.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly IHttpClientFactory _factory;

        public EnrollmentController(IHttpClientFactory factory)
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
            var response = await GetClient().GetAsync("api/enrollment");

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode);

            var enrollments = await response.Content
                .ReadFromJsonAsync<List<Enrollment>>();

            return View(enrollments ?? new List<Enrollment>());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Enrollment model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await GetClient()
                .PostAsJsonAsync("api/enrollment", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Could not enrol in this event.");
                return View(model);
            }

            return RedirectToAction("Index", "Event");
        }
    }
}
