using Backend.API.DTOs;
using Backend.API.Models;

namespace Backend.API.Services.Contracts
{
    public interface IOpenWeatherService
    {
        Task<WeatherOverviewDto?> GetWeatherOverview(OpenWeatherParamsModel openWeatherParamsModel);
    }
}