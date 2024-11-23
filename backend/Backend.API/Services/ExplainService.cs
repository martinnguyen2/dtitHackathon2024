using Backend.API.DTOs;
using Backend.API.Services.Contracts;
using Backend.Common.Models;
using Backend.Common.Utils;

namespace Backend.API.Services;

public class ExplainService : IExplainService
{
    private IPythonExecuteService myPythonExecuteService;

    public ExplainService(IPythonExecuteService pythonExecuteService)
    {
        myPythonExecuteService = pythonExecuteService;
    }

    public async Task<string> Explain(PromptDto userPrompt)
    {
        Dataset? dataset = DatasetFinder.Find(userPrompt.Dataset);

        if (dataset == null)
        {
            return "Dataset was not found!";
        }

        string arguments = $"explain \"" + dataset.Path + "\"";
        
        return await myPythonExecuteService.Execute("main.py", arguments);
    }
}