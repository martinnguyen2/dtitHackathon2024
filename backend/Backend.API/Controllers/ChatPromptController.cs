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
        public ActionResult<string> SubmitPrompt(PromptDto promptDto) 
        {
            if (promptDto == null) 
            {
                return BadRequest();
            }
            
            return _chatPromptService.Submit(promptDto);
        }
    }
}