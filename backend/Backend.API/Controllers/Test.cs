using Backend.API.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Test : ControllerBase
    {
        private readonly IPythonExecuteService myPythonExecuteService;

        public Test(IPythonExecuteService pythonExecuteService)
        {
            myPythonExecuteService = pythonExecuteService;
        }
        
        [HttpGet]
        public ActionResult<string> GetPython()
        {
            return myPythonExecuteService.Execute();
        }
    }
}
