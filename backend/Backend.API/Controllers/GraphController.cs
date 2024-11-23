using Backend.API.Services.Contracts;
using Backend.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraphController : ControllerBase
    {
        private IGraphService myGraphService;

        public GraphController(IGraphService graphService)
        {
            myGraphService = graphService;
        }

        [HttpGet]
        public ActionResult<List<GraphData>> GetGraph([FromQuery]ResponseDataset dataset)
        {
            return myGraphService.Get(dataset);
        }
    }
}
