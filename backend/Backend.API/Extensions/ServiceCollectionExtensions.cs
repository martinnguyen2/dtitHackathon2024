using Backend.API.Services;
using Backend.API.Services.Contracts;

namespace Backend.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IChatPromptService, ChatPromptService>();

        return services;
    }
}