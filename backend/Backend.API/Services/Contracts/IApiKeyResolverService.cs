namespace Backend.API.Services.Contracts;

public interface IApiKeyResolverService
{
    string? GetOpenAIKey();
    string? GetOWKey();
    string? GetGeoCodeKey();
}