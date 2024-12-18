using Backend.Common.Models;

namespace Backend.API.DTOs
{
    public class PromptDto
    {
        required public string Prompt { get; set; }
        public string? CacheId { get; set; }
        required public bool IsExpert { get; set; }
        public int PredictorModel { get; set; }
        public ResponseDataset? Dataset { get; set; }
    }
}