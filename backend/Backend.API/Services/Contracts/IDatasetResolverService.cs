using Backend.Common.Models;

namespace Backend.API.Services.Contracts;

public interface IDatasetResolverService
{
    public Task<ResponseDataset> ResolveDataset(string prompt);
}