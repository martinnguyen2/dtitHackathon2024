using Backend.API.DTOs;
using Backend.API.Enums;
using Backend.API.Services.Contracts;
using Backend.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatPromptController : ControllerBase
    {
        private readonly IChatPromptService _chatPromptService;
        private readonly IPromptToActionResolverService _promptToActionResolverService;

        public ChatPromptController(IChatPromptService chatPromptService, IPromptToActionResolverService promptToActionResolverService)
        {
            _chatPromptService = chatPromptService;
            _promptToActionResolverService = promptToActionResolverService;
        }
        
        [HttpPost("submit")]
        public async Task<ActionResult<object>> SubmitPrompt(PromptDto promptDto) 
        {
            if (promptDto == null) 
            {
                return BadRequest();
            }

            if (promptDto.CacheId == string.Empty || promptDto.CacheId == "-1")
            {
                promptDto.CacheId = null;
            }
            
            var action = await ResolveActionFromPrompt(promptDto.Prompt);
            
            return await _chatPromptService.Submit(promptDto, action);
        }
        
        private async Task<ActionEnum> ResolveActionFromPrompt(string userPrompt)
        {
            return await _promptToActionResolverService.ResolveActionFromPrompt(userPrompt);
        }
    }
}