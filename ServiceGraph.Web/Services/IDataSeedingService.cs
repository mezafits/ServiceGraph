using ServiceGraph.Common;

namespace ServiceGraph.Web.Services
{
    public interface IDataSeedingService
    {
        Task SeedSampleDataAsync();
    }
}