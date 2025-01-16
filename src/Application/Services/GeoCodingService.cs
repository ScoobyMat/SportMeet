using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

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
            Console.WriteLine($"Requesting geocode for: {address}");

            var url = $"https://geocode.search.hereapi.com/v1/geocode?q={address}&apiKey={_apiKey}";
            var client = new HttpClient();

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
                    else
                    {
                        Console.WriteLine("No coordinates found for the given address.");
                    }
                }
                else
                {
                    Console.WriteLine($"Request failed with status code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request failed: {e.Message}");
            }

            return null;

        }
    }
}
