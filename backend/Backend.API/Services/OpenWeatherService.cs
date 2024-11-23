using System.Runtime.InteropServices;
using System.Text.Json;
using Backend.API.DTOs;
using Backend.API.Models;
using Backend.API.Services.Contracts;

namespace Backend.API.Services
{
    public class OpenWeatherService : IOpenWeatherService
    {
        private readonly IGeoCodeResolverService _geoCodeResolverService;
        private readonly IApiKeyResolverService _apiKeyResolverService;
        public OpenWeatherService(IGeoCodeResolverService geoCodeResolverService, IApiKeyResolverService apiKeyResolverService)
        {
            _apiKeyResolverService = apiKeyResolverService;
            
            _geoCodeResolverService = geoCodeResolverService;
        }

        public async Task<WeatherOverviewDto?> GetWeatherOverview(OpenWeatherParamsModel openWeatherParamsModel)
        {

            var cityName = openWeatherParamsModel.CityName;

            if (cityName != null)
            {
                var latLong = await _geoCodeResolverService.GetLatLongByCityName(cityName);

                if (latLong != null)
                {
                    openWeatherParamsModel.Latitude = latLong.Latitude;
                    openWeatherParamsModel.Longitude = latLong.Longitude;
                }
            }

            var lat = openWeatherParamsModel.Latitude;
            var lon = openWeatherParamsModel.Longitude;
            var apiKey = _apiKeyResolverService.GetOWKey();
            var url = $"https://api.openweathermap.org/data/3.0/onecall/overview?lat={lat?.Replace(',', '.')}&lon={lon?.Replace(',', '.')}&appid={apiKey}";

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(url);

                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    System.Console.WriteLine(jsonResponse);
                    System.Console.WriteLine(url);

                    // deserialize response
                    var deserialized = JsonSerializer.Deserialize<WeatherOverviewDto>(jsonResponse);


                    if (deserialized != null)
                    {
                        deserialized.CityName = openWeatherParamsModel.CityName;
                        return deserialized;
                    }

                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}