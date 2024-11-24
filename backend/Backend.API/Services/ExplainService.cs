using System.Text.Json;
using Backend.API.DTOs;
using Backend.API.Services.Contracts;
using Backend.Common.Models;
using Backend.Common.Utils;

namespace Backend.API.Services;

public class ExplainService : IExplainService
{
    private readonly IPythonExecuteService myPythonExecuteService;

    public ExplainService(IPythonExecuteService pythonExecuteService)
    {
        myPythonExecuteService = pythonExecuteService;
    }

    public async Task<ExplainServiceResponse> Explain(PromptDto promptDto)
    {
        Dataset? dataset = DatasetFinder.Find(promptDto.Dataset);

        if (dataset == null)
        {
            return new ExplainServiceResponse()
            {
                Type = "explain",
                TextOutput = "Dataset was not found!",
                CacheId = "-1"
            };
        }

        string arguments;

        if (promptDto.CacheId == null)
        {
            arguments = $"--action explain " +
                $"--dataset_path \"{dataset.Path}\" " +
                $"--prompt \"{promptDto.Prompt}\"" +
                (promptDto.IsExpert ? " --isExpert True" : "");
        }
        else
        {
            arguments = $"--cacheId \"{promptDto.CacheId}\" " + 
                $"--dataset_path \"{dataset.Path}\" " +
                $"--prompt \"{promptDto.Prompt}\"" +
                (promptDto.IsExpert ? " --isExpert True" : "");
        }

        ExplainScriptResponseDto? dto;

        try
        {
            dto = JsonSerializer.Deserialize<ExplainScriptResponseDto>(await myPythonExecuteService.Execute("main.py", arguments));
        }
        catch (Exception exception)
        {
            Console.WriteLine($"An error occured! {exception.Message}");
            return new ExplainServiceResponse()
            {
                Type = "explain",
                TextOutput = "There was an issue while getting the response from OpenAI.",
                CacheId = "-1"
            };
        }

        if (dto == null)
        {
            return new ExplainServiceResponse()
            {
                Type = "explain",
                TextOutput = "There was an issue while getting the response from OpenAI.",
                CacheId = "-1"
            };
        }

        ExplainServiceResponse response = new ExplainServiceResponse()
        {
            Type = "explain",
            TextOutput = dto.text_output,
            CacheId = dto.cacheId
        };
        
        return response;
    }
    
    public async Task<ExplainServiceResponse> Analyze(PromptDto promptDto)
    {
        Dataset? dataset = DatasetFinder.Find(promptDto.Dataset);

        if (dataset == null)
        {
            return new ExplainServiceResponse()
            {
                Type = "analyze",
                TextOutput = "Dataset was not found!",
                CacheId = "-1"
            };
        }

        string arguments;

        if (promptDto.CacheId == null)
        {
            arguments = $"--action analyze " +
                $"--dataset_path \"{dataset.Path}\" " +
                $"--prompt \"{promptDto.Prompt}\"" +
                (promptDto.IsExpert ? " --isExpert True" : "");
        }
        else
        {
            arguments = $"--action analyze --cacheId \"{promptDto.CacheId}\" " + 
                $"--dataset_path \"{dataset.Path}\" " +
                $"--prompt \"{promptDto.Prompt}\"" +
                (promptDto.IsExpert ? " --isExpert True" : "");
        }

        ExplainScriptResponseDto? dto;

        try
        {
            dto = JsonSerializer.Deserialize<ExplainScriptResponseDto>(await myPythonExecuteService.Execute("main.py", arguments));
        }
        catch (Exception exception)
        {
            Console.WriteLine($"An error occured! {exception.Message}");
            return new ExplainServiceResponse()
            {
                Type = "analyze",
                TextOutput = "There was an issue while getting the response from OpenAI.",
                CacheId = "-1"
            };
        }

        if (dto == null)
        {
            return new ExplainServiceResponse()
            {
                Type = "analyze",
                TextOutput = "There was an issue while getting the response from OpenAI.",
                CacheId = "-1"
            };
        }

        ExplainServiceResponse response = new ExplainServiceResponse()
        {
            Type = "analyze",
            TextOutput = dto.text_output,
            CacheId = dto.cacheId
        };
        
        return response;
    }
}