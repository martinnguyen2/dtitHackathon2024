using Backend.API.DTOs;
using Backend.API.Enums;
using Backend.Common.Models;

namespace Backend.API.Services.Contracts
{
    public interface IChatPromptService
    {
        Task<ExplainServiceResponse> Submit(PromptDto promptDto, ActionEnum action);
    }
}