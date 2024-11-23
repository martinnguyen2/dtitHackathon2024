using Backend.API.DTOs;
using Backend.API.Services.Contracts;
using OpenAI.Chat;

namespace Backend.API.Services;

public class ChatPromptService : IChatPromptService
{
    public IApiKeyResolverService _apiKeyResolverService { get; }

    public ChatPromptService(IApiKeyResolverService apiKeyResolverService)
    {
        _apiKeyResolverService = apiKeyResolverService;
    }
    
    public async Task<string> Submit(PromptDto promptDto)
    {
        var apiKey = _apiKeyResolverService.GetOpenAIKey();
        
        if (apiKey == null)
        {
            throw new Exception("OpenAI API key not set in environment variables");
        }
        
        ChatClient client = new ChatClient(model: "gpt-4", apiKey);

        var chatCompletion = await client.CompleteChatAsync($"Say reply to this prompt {promptDto.Prompt}");
        
        return $"{chatCompletion.Value.Content[0].Text}";
    }
}