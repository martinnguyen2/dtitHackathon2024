using Backend.API.Models;

namespace Backend.API.Services.Contracts
{
    public interface IGeoCodeResolverService
    {
        Task<LatLongModel?> GetLatLongByCityName(string cityName);
    }
}