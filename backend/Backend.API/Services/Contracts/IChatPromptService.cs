using Backend.API.DTOs;
using Backend.API.Enums;

namespace Backend.API.Services.Contracts;

public interface IChatPromptService
{
    Task<string> Submit(PromptDto promptDto, ActionEnum action);
}