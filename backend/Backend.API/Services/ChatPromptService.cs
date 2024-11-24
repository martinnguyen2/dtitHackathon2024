using Backend.API.DTOs;
using Backend.API.Enums;
using Backend.API.Services.Contracts;
using Backend.Common.Models;
namespace Backend.API.Services;

public class ChatPromptService : IChatPromptService
{
    private readonly IExplainService _explainService;
    private readonly IVisualizeService _visualizeService;
    public IApiKeyResolverService _apiKeyResolverService { get; }

    public ChatPromptService(IApiKeyResolverService apiKeyResolverService, IExplainService explainService, IVisualizeService visualizeService)
    {
        _explainService = explainService;
        _visualizeService = visualizeService;
        _apiKeyResolverService = apiKeyResolverService;
    }
    
    public async Task<object> Submit(PromptDto promptDto, ActionEnum action)
    {
        switch (action)
        {
            case ActionEnum.EXPLAIN:
                return await _explainService.Explain(promptDto);
            case ActionEnum.VISUALIZE:
                var response = await _visualizeService.Visualize(promptDto);

                if (response == null)
                {
                    return new ExplainServiceResponse()
                    {
                        Type = "explain",
                        TextOutput = "Could not load visualization data from OpenAI.",
                        CacheId = "-1",
                    };
                }

                return response;
            case ActionEnum.ANALYZE:
                return await _explainService.Analyze(promptDto);
            default:
                break;
        }
        
        return new ExplainServiceResponse()
        {
            Type = "explain",
            TextOutput = "This type of prompt is not supported.",
            CacheId = "-1"
        };
    }
}