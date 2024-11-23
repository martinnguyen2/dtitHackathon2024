using Backend.API.DTOs;

namespace Backend.API.Services.Contracts;

public interface IExplainService
{
    Task<string> Explain(PromptDto promptDto);
}