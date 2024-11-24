using Backend.API.Services;
using Backend.API.Services.Contracts;

namespace Backend.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IOpenWeatherService, OpenWeatherService>();
        services.AddScoped<IChatPromptService, ChatPromptService>();
        services.AddScoped<IApiKeyResolverService, ApiKeyResolverService>();
        services.AddScoped<IPromptToActionResolverService, PromptToActionResolverService>();
        services.AddScoped<IExplainService, ExplainService>();
        services.AddScoped<IVisualizeService, VisualizeService>();
        services.AddScoped<IGeoCodeResolverService, GeoCodeResolverService>();
        services.AddScoped<IDatasetResolverService, DatasetResolverService>();

        return services;
    }
}