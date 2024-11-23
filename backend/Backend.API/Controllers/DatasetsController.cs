using Backend.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatasetsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<ResponseDataset>> GetDatasets()
        {
            List<ResponseDataset> datasets = [];

            string[] files = Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "Data"));

            foreach (string file in files)
            {
                ResponseDataset dataset = new()
                {
                    Name = Path.GetFileNameWithoutExtension(file)
                };

                datasets.Add(dataset);
            }
            
            return datasets;
        }
    }
}
