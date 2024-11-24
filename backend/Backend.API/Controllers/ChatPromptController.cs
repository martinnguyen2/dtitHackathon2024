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
        private readonly IDatasetResolverService _datasetResolverService;
        private readonly IPromptToActionResolverService _promptToActionResolverService;

        public ChatPromptController(IChatPromptService chatPromptService, IDatasetResolverService datasetResolverService, IPromptToActionResolverService promptToActionResolverService)
        {
            _chatPromptService = chatPromptService;
            _datasetResolverService = datasetResolverService;
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

            if (promptDto.Dataset == null)
            {
                promptDto.Dataset = await _datasetResolverService.ResolveDataset(promptDto.Prompt);
            }
            
            var action = await ResolveActionFromPrompt(promptDto.Prompt);
            
            dynamic resultJson = await _chatPromptService.Submit(promptDto, action);
            resultJson.Dataset = promptDto.Dataset.Name;
            return resultJson;
        }
        
        private async Task<ActionEnum> ResolveActionFromPrompt(string userPrompt)
        {
            return await _promptToActionResolverService.ResolveActionFromPrompt(userPrompt);
        }
    }
}