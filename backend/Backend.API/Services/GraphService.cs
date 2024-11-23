using Backend.API.Services.Contracts;
using Backend.Common.Models;

namespace Backend.API.Services;

public class GraphService : IGraphService
{
    public List<GraphData> Get(ResponseDataset dataset)
    {
        // Hardcoded, ignore the dataset
        List<GraphData> graph = 
        [
            new GraphData
            {
                Label = "January",
                Value = "78"
            },
            new GraphData
            {
                Label = "February",
                Value = "93"
            },new GraphData
            {
                Label = "March",
                Value = "19"
            },
            new GraphData
            {
                Label = "April",
                Value = "56"
            },
            new GraphData
            {
                Label = "May",
                Value = "32"
            },
            new GraphData
            {
                Label = "June",
                Value = "78"
            },
            new GraphData
            {
                Label = "July",
                Value = "7"
            },
            new GraphData
            {
                Label = "August",
                Value = "0"
            },
            new GraphData
            {
                Label = "September",
                Value = "196"
            },
            new GraphData
            {
                Label = "October",
                Value = "36"
            },
            new GraphData
            {
                Label = "November",
                Value = "71"
            },
            new GraphData
            {
                Label = "December",
                Value = "100"
            }
        ];

        return graph;
    }
}
