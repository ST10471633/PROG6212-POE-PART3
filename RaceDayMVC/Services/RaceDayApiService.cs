
using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace RaceDayMVC.Service
{
    public class ApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiService(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public HttpClient GetClient()
        {
            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            if (client.BaseAddress == null)
            {
                client.BaseAddress =
                    new Uri("https://localhost:7011/");
            }

            var context = _httpContextAccessor.HttpContext;

            if (context != null &&
                context.Request.Headers.TryGetValue("Cookie", out var cookie))
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation(
                    "Cookie", cookie.ToString());
            }

            return client;
        }
    }
}
