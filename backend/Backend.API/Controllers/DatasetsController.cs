using Backend.Common.Models;
using Backend.Common.Utils;
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
                ResponseDataset dataset = new ResponseDataset
                {
                    Name = Path.GetFileNameWithoutExtension(file)
                };

                datasets.Add(dataset);
            }
            
            return datasets;
        }

        [HttpPost]
        public ActionResult<string> PostDataset(IFormFile file)
        {
            FileUploadResponse response = UploadHandler.Upload(file);

            if (response.Status == FileUploadResponse.StatusEnum.Error)
            {
                return BadRequest(response.Text);
            }

            return Ok(response.Text);
        }
    }
}
