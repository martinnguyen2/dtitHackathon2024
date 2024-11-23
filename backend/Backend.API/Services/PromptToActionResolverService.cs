using Backend.API.Enums;
using Backend.API.Services.Contracts;
using OpenAI.Chat;

namespace Backend.API.Services;

public class PromptToActionResolverService : IPromptToActionResolverService
{
    private readonly IApiKeyResolverService _apiKeyResolverService;
    private readonly IChatPromptService _chatPromptService;

    public PromptToActionResolverService(IApiKeyResolverService apiKeyResolverService, IChatPromptService chatPromptService)
    {
        _apiKeyResolverService = apiKeyResolverService;
        _chatPromptService = chatPromptService;
    }
    
    public async Task<ActionEnum> ResolveActionFromPrompt(string userPrompt)
    {
        var actionEnums = Enum.GetValues<ActionEnum>();
        var actionEnumsString = string.Join(", ", actionEnums);

        // Use the prompt to analyze and deduce the action
        string masterPrompt = $"Determine which one of allowed actions specified in context will user prompt be requesting by its semantic. The returned value can be just enum name, nothing else.";
        string context = $"Available enum values are: {actionEnumsString}";
        
        var aiResult = await AnalyzePrompt(masterPrompt, context, userPrompt);
        
        ActionEnum resolvedAction = ActionEnum.UNKNOWN; // Default to UNKNOWN
    
        if (Enum.TryParse(aiResult, true, out ActionEnum parsedEnum) && Enum.IsDefined(typeof(ActionEnum), parsedEnum))
        {
            resolvedAction = parsedEnum;
        }
        else
        {
            if (aiResult.ToLower().Contains("explain"))
            {
                resolvedAction = ActionEnum.EXPLAIN;
            }
            else if (aiResult.ToLower().Contains("visualize"))
            {
                resolvedAction = ActionEnum.VISUALIZE;
            }
        }

        return resolvedAction;
    }
    
    private async Task<string> AnalyzePrompt(string masterPrompt, string context, string userPrompt)
    {
        var apiKey = _apiKeyResolverService.GetOpenAIKey();
        
        if (apiKey == null)
        {
            throw new Exception("OpenAI API key not set in environment variables");
        }
        
        ChatClient client = new ChatClient(model: "gpt-4", apiKey);
        
        string combinedPrompt = $"Analyze the following scenario:\n\nMaster Prompt: {masterPrompt}\nContext: {context}\nUser Prompt: {userPrompt}";
        var chatCompletion = await client.CompleteChatAsync(combinedPrompt);
        string analysisText = chatCompletion.Value.Content[0].Text;

        return analysisText;

    }
}