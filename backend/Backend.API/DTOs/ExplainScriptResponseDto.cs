namespace Backend.API.DTOs
{
    public class ExplainScriptResponseDto
    {
        required public string text_output { get; set; }
        required public string cacheId { get; set; }
    }
}
