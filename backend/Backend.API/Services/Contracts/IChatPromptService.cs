using Backend.API.DTOs;

namespace Backend.API.Services.Contracts;

public interface IChatPromptService
{
    string Submit(PromptDto promptDto);
}