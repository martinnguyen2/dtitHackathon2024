using Backend.API.DTOs;
using Backend.Common.Models;

namespace Backend.API.Services.Contracts
{
    public interface IVisualizeService
    {
        Task<VisualizeServiceResponse?> Visualize(PromptDto promptDto);
    }
}