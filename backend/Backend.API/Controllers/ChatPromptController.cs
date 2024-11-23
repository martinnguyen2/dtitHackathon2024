using Backend.API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatPromptController : ControllerBase
    {
        [HttpPost("submit")]
        public ActionResult<string> SubmitPrompt(PromptDto promptDto) 
        {
            /* in body
            obj: {
                public string prompt {get; set;}
            }
            */ 

            if (promptDto == null) 
            {
                return BadRequest();
            }
            
            return promptDto.Prompt;
        }
    }
}