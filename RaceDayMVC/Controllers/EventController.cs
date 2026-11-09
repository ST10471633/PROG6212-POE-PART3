
using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Models;
using RaceDayMVC.Service;
using System.Net.Http.Json;

namespace RaceDayMVC.Controllers
{
    public class EventController : Controller
    {
        private readonly ApiService _apiService;

        public EventController(ApiService apiService)
        {
            _apiService = apiService;
        }

        private HttpClient GetClient()
        {
            return _apiService.GetClient();
        }

        public async Task<IActionResult> Index()
        {
            var client = GetClient();

            var events = await client.GetFromJsonAsync<List<Event>>(
                "api/event");

            return View(events ?? new List<Event>());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await GetClient().PostAsJsonAsync(
                "api/event", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("",
                    "Could not create the event. Please check your login and details.");

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await GetClient().GetAsync($"api/event/{id}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var model = await response.Content.ReadFromJsonAsync<Event>();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event model)
        {
            if (id != model.EventID)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            var response = await GetClient().PutAsJsonAsync(
                $"api/event/{id}", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("",
                    "Could not update the event.");

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await GetClient().DeleteAsync(
                $"api/event/{id}");

            if (!response.IsSuccessStatusCode)
                return BadRequest("Could not delete the event.");

            return RedirectToAction(nameof(Index));
        }
    }
}
