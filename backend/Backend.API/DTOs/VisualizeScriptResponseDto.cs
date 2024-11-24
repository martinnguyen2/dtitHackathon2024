namespace Backend.API.DTOs
{
    public class VisualizeScriptResponseDto
    {
        public required string type { get; set; }
        public required string graphTitle { get; set; }
        public required VisualizeScriptResponseDtoData data { get; set; }
        public VisualizeScriptResponseDtoPredictedData? predictedData { get; set; }
    }

    public class VisualizeScriptResponseDtoData
    {
        public required string[] labels { get; set; }
        public required double[] values { get; set; }
        public required string yLabel { get; set; }
        public required string xLabel { get; set; }
    }

    public class VisualizeScriptResponseDtoPredictedData
    {
        public string[]? labels { get; set; }
        public double[]? values { get; set; }
        public string? yLabel { get; set; }
        public string? xLabel { get; set; }
    }
}
