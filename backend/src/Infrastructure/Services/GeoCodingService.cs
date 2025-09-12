using System.Net.Http.Json;
using System.Text.Json;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class GeoCodingService : IGeoCodingService
    {
        private readonly string _apiKey;

        public GeoCodingService(IConfiguration configuration)
        {
            _apiKey = configuration["HereAPI:ApiKey"];
        }

        public async Task<Event?> GetCoordinatesAsync(Event newEvent)
        {
            var address = $"{Uri.EscapeDataString(newEvent.Address)}+{Uri.EscapeDataString(newEvent.City)}";
            var url = $"https://geocode.search.hereapi.com/v1/geocode?q={address}&apiKey={_apiKey}";

            using var client = new HttpClient();

            try
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                    if (result.TryGetProperty("items", out var items) && items.GetArrayLength() > 0)
                    {
                        var position = items[0].GetProperty("position");
                        newEvent.Latitude = position.GetProperty("lat").GetDouble();
                        newEvent.Longitude = position.GetProperty("lng").GetDouble();
                        return newEvent;
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"[GeoCoding] Request failed: {e.Message}");
            }

            return null;
        }
    }
}
