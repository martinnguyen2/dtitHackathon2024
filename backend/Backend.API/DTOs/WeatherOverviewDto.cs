using System.Text.Json.Serialization;

namespace Backend.API.DTOs
{
    public class WeatherOverviewDto
    {
        public string? CityName { get; set; }

        [JsonPropertyName("lat")]
        public double Latitude { get; set; }

        [JsonPropertyName("lon")]
        public double Longitude { get; set; }

        [JsonPropertyName("tz")]
        public string TimeZone { get; set; } = string.Empty;

        [JsonPropertyName("date")]
        public string Date { get; set; } = string.Empty;

        [JsonPropertyName("units")]
        public string Units { get; set; } = string.Empty;

        [JsonPropertyName("weather_overview")]
        public string WeatherOverview { get; set; } = string.Empty;

    }
}