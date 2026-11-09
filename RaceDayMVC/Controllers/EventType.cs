
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using RaceDayMVC.Models;

namespace RaceDayMVC.Controllers
{
    public class EventTypeController : Controller
    {
        private readonly IHttpClientFactory _factory;

        public EventTypeController(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        private HttpClient GetClient()
        {
            var client = _factory.CreateClient("RaceDayAPI");

            if (Request.Headers.ContainsKey("Cookie"))
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation(
                    "Cookie", Request.Headers.Cookie.ToString());
            }

            return client;
        }

        public async Task<IActionResult> Index()
        {
            var response = await GetClient().GetAsync("api/eventtype");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Unable to load event types. Please log in and try again.";
                return View(new List<EventType>());
            }

            var eventTypes =
                await response.Content.ReadFromJsonAsync<List<EventType>>();

            return View(eventTypes ?? new List<EventType>());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventType model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await GetClient()
                .PostAsJsonAsync("api/eventtype", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    "", "Unable to create the event type. Check your login and details.");

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await GetClient()
                .GetAsync($"api/eventtype/{id}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var model =
                await response.Content.ReadFromJsonAsync<EventType>();

            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventType model)
        {
            if (id != model.EventTypeID)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            var response = await GetClient()
                .PutAsJsonAsync($"api/eventtype/{id}", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Unable to update the event type.");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await GetClient()
                .DeleteAsync($"api/eventtype/{id}");

            if (!response.IsSuccessStatusCode)
                return BadRequest("Unable to delete the event type.");

            return RedirectToAction(nameof(Index));
        }
    }
}
