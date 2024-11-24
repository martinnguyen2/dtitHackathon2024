namespace Backend.Common.Models
{
    public class ExplainServiceResponse
    {
        public required string Type { get; set; }
        public required string TextOutput { get; set; }
        public required string CacheId { get; set; }
    }
}
