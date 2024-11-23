using Backend.API.DTOs;
using Backend.API.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatPromptController : ControllerBase
    {
        private readonly IChatPromptService _chatPromptService;

        public ChatPromptController(IChatPromptService chatPromptService)
        {
            _chatPromptService = chatPromptService;
        }
        
        [HttpPost("submit")]
        public async Task<ActionResult<string>> SubmitPrompt(PromptDto promptDto) 
        {
            if (promptDto == null) 
            {
                return BadRequest();
            }
            
            return await _chatPromptService.Submit(promptDto);
        }
    }
}