using Backend.API.Services.Contracts;

namespace Backend.API.Services;

public class ApiKeyResolverService : IApiKeyResolverService
{
    public string? GetOpenAIKey()
    {
        return Environment.GetEnvironmentVariable("OPENAI_API_KEY");
    }
}