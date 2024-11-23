using Backend.Common.Models;

namespace Backend.API.Services.Contracts
{
    public interface IGraphService
    {
        List<GraphData> Get(ResponseDataset dataset);
    }
}
