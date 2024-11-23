using Backend.API.DTOs;
using Backend.API.Services.Contracts;

namespace Backend.API.Services;

public class ChatPromptService : IChatPromptService
{
    public string Submit(PromptDto promptDto)
    {
        return promptDto.Prompt;
    }
}