using System.Text.Json;
using Backend.API.DTOs;
using Backend.API.Models;
using Backend.API.Services.Contracts;

namespace Backend.API.Services
{

    public class GeoCodeResolverService : IGeoCodeResolverService
    {
        private readonly IApiKeyResolverService _apiKeyResolverService;
        public GeoCodeResolverService(IApiKeyResolverService apiKeyResolverService)
        {
            _apiKeyResolverService = apiKeyResolverService;

        }
        public async Task<LatLongModel?> GetLatLongByCityName(string cityName)
        {
            var apiKey = _apiKeyResolverService.GetGeoCodeKey();

            string url = $"https://api.opencagedata.com/geocode/v1/json?q={cityName}&key={apiKey}";

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(url);
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // deserialize response
                    var deserialized = JsonSerializer.Deserialize<GeoCodeResponseDto>(jsonResponse);

                    if (deserialized?.Results != null && deserialized.Results.Count > 0)
                    {
                        var geometry = deserialized.Results[0].GeometryDto;
                        return new LatLongModel
                        {
                            Latitude = geometry.Latitude.ToString(),
                            Longitude = geometry.Longitude.ToString()
                        };
                    }

                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}