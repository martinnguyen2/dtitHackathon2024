using Backend.API.DTOs;
using Backend.Common.Models;

namespace Backend.API.Services.Contracts
{
    public interface IExplainService
    {
        Task<ExplainServiceResponse> Explain(PromptDto promptDto);
    }
}
