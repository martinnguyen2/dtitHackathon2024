using Backend.Common.Models;

namespace Backend.Common.Utils
{
    public static class DatasetFinder
    {
        public static Dataset? Find(ResponseDataset dataset)
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Data", dataset.Name, ".csv");

            if (!Directory.Exists(path))
            {
                return null;
            }

            return new Dataset()
            {
                Name = dataset.Name,
                Path = path
            };
        }
    }
}
