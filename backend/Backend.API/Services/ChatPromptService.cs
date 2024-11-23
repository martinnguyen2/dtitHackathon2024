using Backend.API.DTOs;
using Backend.API.Enums;
using Backend.API.Services.Contracts;
using Newtonsoft.Json;
using OpenAI.Chat;
using Formatting = System.Xml.Formatting;

namespace Backend.API.Services;

public class ChatPromptService : IChatPromptService
{
    private readonly IExplainService _explainService;
    public IApiKeyResolverService _apiKeyResolverService { get; }

    public ChatPromptService(IApiKeyResolverService apiKeyResolverService, IExplainService explainService)
    {
        _explainService = explainService;
        _apiKeyResolverService = apiKeyResolverService;
    }
    
    public async Task<string> Submit(PromptDto promptDto, ActionEnum action)
    {
        switch (action)
        {
            case ActionEnum.EXPLAIN:
                return await _explainService.Explain(promptDto);
            case ActionEnum.VISUALIZE:
                // TODO
                break;
            default:
                break;
        }
        
        return "IDK what to do with this prompt";
    }
}