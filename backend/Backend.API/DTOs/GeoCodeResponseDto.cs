using System.Text.Json.Serialization;

namespace Backend.API.DTOs
{
    public class GeometryDto
    {
        [JsonPropertyName("lat")]
        public double Latitude { get; set; }

        [JsonPropertyName("lng")]
        public double Longitude { get; set; }
    }

    public class ResultDto
    {
        [JsonPropertyName("geometry")]
        public required GeometryDto GeometryDto { get; set; }
    }

    public class GeoCodeResponseDto
    {
        [JsonPropertyName("results")]
        public required List<ResultDto> Results { get; set; }
    }
}