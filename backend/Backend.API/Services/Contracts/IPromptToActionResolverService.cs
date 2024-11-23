using Backend.API.Enums;

namespace Backend.API.Services.Contracts;

public interface IPromptToActionResolverService
{
    Task<ActionEnum> ResolveActionFromPrompt(string userPrompt);
}