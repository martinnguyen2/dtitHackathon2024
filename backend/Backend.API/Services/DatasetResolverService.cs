using Backend.API.Services.Contracts;
using Backend.Common.Models;
using OpenAI.Chat;

namespace Backend.API.Services;

public class DatasetResolverService : IDatasetResolverService
{
    private readonly IApiKeyResolverService _apiKeyResolverService;

    public DatasetResolverService(IApiKeyResolverService apiKeyResolverService)
    {
        _apiKeyResolverService = apiKeyResolverService;
    }

    public async Task<ResponseDataset> ResolveDataset(string userPrompt)
    {
        List<ResponseDataset> datasets = [];

        string[] files = Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "Data"));

        foreach (string file in files)
        {
            ResponseDataset dataset = new ResponseDataset
            {
                Name = Path.GetFileNameWithoutExtension(file)
            };

            datasets.Add(dataset);
        }
            
        var availableDatasets = datasets.Select(x => x.Name).ToList();
        
        var availableDatasetsString = string.Join(", ", availableDatasets.Select(x => $"\"{x}\""));
        string masterPrompt = $"Determine which one of allowed actions specified in context will user prompt be requesting by its semantic. The returned value can be just enum name, nothing else.";
        string context = $"Available dataset files are: {availableDatasetsString}. The user prompt is expecting to open some dataset. If you find any possible match, then return the name of the dataset - only the name, nothing else.";
        
        Console.WriteLine(availableDatasetsString);
        
        var aiResult = await AnalyzePrompt(masterPrompt, context, userPrompt);

        if (availableDatasets.Contains(aiResult))
        {
            // traverse the datasets and find the one that matches the result
            foreach (var dataset in datasets)
            {
                if (dataset.Name == aiResult)
                {
                    return dataset;
                }
            }
        }
        
        throw new Exception("Sorry, I could not find any dataset that matches the user prompt.");
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