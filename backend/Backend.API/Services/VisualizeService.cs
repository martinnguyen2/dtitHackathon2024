using System.Text.Json;
using Backend.API.DTOs;
using Backend.API.Services.Contracts;
using Backend.Common.Models;
using Backend.Common.Utils;

namespace Backend.API.Services
{
    public class VisualizeService : IVisualizeService
    {
        private readonly IPythonExecuteService myPythonExecuteService;

        public VisualizeService(IPythonExecuteService pythonExecuteService)
        {
            myPythonExecuteService = pythonExecuteService;
        }

        public async Task<VisualizeServiceResponse?> Visualize(PromptDto promptDto)
        {
            Dataset? dataset = DatasetFinder.Find(promptDto.Dataset);

            if (dataset == null)
            {
                return null;
            }

            string arguments = $"--action visualize " +
                $"--dataset_path \"{dataset.Path}\" " +
                $"--prompt \"{promptDto.Prompt}\"" +
                (promptDto.PredictorModel == 1 ? " --predictorModel svm" : "");

            VisualizeScriptResponseDto? dto;

            try
            {
                var a = await myPythonExecuteService.Execute("main.py", arguments);
                Console.WriteLine(a);
                dto = JsonSerializer.Deserialize<VisualizeScriptResponseDto>(a);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"An error occured! {exception.Message}");
                return null;
            }

            if (dto == null)
            {
                return null;
            }

            List<GraphData> graphDatas = [];
            List<GraphData>? predictedGraphDatas = [];

            for (int i = 0; i < dto.data.labels.Length; i++)
            {
                graphDatas.Add(
                    new GraphData
                    {
                        Label = dto.data.labels[i],
                        Value = dto.data.values[i]
                    }
                );
            }

            if (dto.predictedData != null)
            {
                for (int i = 0; i < dto.predictedData?.labels?.Length; i++)
                {
                    predictedGraphDatas.Add(
                        new GraphData
                        {
                            Label = dto.predictedData.labels[i],
                            Value = dto.predictedData.values![i]
                        }
                    );
                }
            }

            VisualizeServiceResponse response = new VisualizeServiceResponse
            {
                Type = "visualize",
                CacheId = "",
                GraphTitle = dto.graphTitle,
                ChartType = dto.type,
                GraphData = graphDatas,
                XLabel = dto.data.xLabel,
                YLabel = dto.data.yLabel,
                PredictedGraphData = predictedGraphDatas ?? null
            };

            return response;
        }
    }
}
