using Backend.API.Models;
using Backend.API.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IPythonExecuteService myPythonExecuteService;
        private readonly IOpenWeatherService _openWeatherService;

        public TestController(IOpenWeatherService openWeatherService, IPythonExecuteService pythonExecuteService)
        {
            myPythonExecuteService = pythonExecuteService;
            _openWeatherService = openWeatherService;
        }

        [HttpGet("GetLatLongByCityName")]
        public async Task<ActionResult> GetLatLongByCityName()
        {
            var result = await _openWeatherService.GetWeatherOverview(new OpenWeatherParamsModel
            {
                CityName = "Bratislava, Slovakia"
            });

            return Ok(result);
        }

        [HttpGet]
        public ActionResult<string> GetPython()
        {
            return myPythonExecuteService.Execute();
        }
    }
}
