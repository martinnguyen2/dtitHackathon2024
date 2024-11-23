using Backend.API.Services.Contracts;

namespace Backend.API.Services;

public class ApiKeyResolverService : IApiKeyResolverService
{
    public string? GetGeoCodeKey()
    {
        return Environment.GetEnvironmentVariable("GEO_CODE_API_KEY");
    }

    public string? GetOpenAIKey()
    {
        return Environment.GetEnvironmentVariable("OPENAI_API_KEY");
    }

    public string? GetOWKey()
    {
        return Environment.GetEnvironmentVariable("OPEN_WEATHER_API_KEY");
    }
}