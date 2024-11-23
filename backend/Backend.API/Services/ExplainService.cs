using Backend.API.Services.Contracts;

namespace Backend.API.Services;

public class ExplainService : IExplainService
{
    public async Task<string> Explain(string userPrompt)
    {
        return "I am explaining content of the dataset";
    }
}