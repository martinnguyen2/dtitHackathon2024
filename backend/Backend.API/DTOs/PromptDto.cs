using Backend.Common.Models;

namespace Backend.API.DTOs
{
    public class PromptDto
    {
        required public string Prompt { get; set; }
        required public ResponseDataset Dataset { get; set; }
    }
}