namespace Backend.Common.Models
{
    public class VisualizeServiceResponse
    {
        public required string Type { get; set; }
        public required string CacheId { get; set; }

        public required string ChartType { get; set; }
        public required List<GraphData> GraphData { get; set; }
        public required string XLabel { get; set; }
        public required string YLabel { get; set; }
    }
}