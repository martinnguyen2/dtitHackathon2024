namespace Backend.API.Services.Contracts;

public interface IExplainService
{
    Task<string> Explain(string userPrompt);
}