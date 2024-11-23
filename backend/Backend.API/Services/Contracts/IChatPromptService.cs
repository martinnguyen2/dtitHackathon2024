using Backend.API.DTOs;

namespace Backend.API.Services.Contracts;

public interface IChatPromptService
{
    Task<string> Submit(PromptDto promptDto);
}