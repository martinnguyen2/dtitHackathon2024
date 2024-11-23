using Backend.API.DTOs;
using Backend.API.Enums;
using Backend.API.Services.Contracts;
using Backend.Common.Models;
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
    
    public async Task<ExplainServiceResponse> Submit(PromptDto promptDto, ActionEnum action)
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
        
        return new ExplainServiceResponse()
        {
            TextOutput = "IDK what to do with this prompt",
            CacheId = "-1"
        };
    }
}