namespace Backend.API.DTOs
{
    public class VisualizeScriptResponseDto
    {
        public required string type { get; set; }
        public required VisualizeScriptResponseDtoData data { get; set; }
    }

    public class VisualizeScriptResponseDtoData
    {
        public required string[] labels { get; set; }
        public required double[] values { get; set; }
        public required string yLabel { get; set; }
        public required string xLabel { get; set; }
    }
}
