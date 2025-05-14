using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.Linq.Expressions;
using System.Net;
using ServiceGraph.Common;

public class CosmosRepository<T> : ICosmosRepository<T> where T : BaseObject
{
    private readonly CosmosClient _cosmosClient;
    private readonly string _databaseName;
    private readonly string _containerId;
    private readonly string _partitionKeyPath = "/pid";
    private Task<Container> _containerTask;

    public CosmosRepository(DatabaseSettings settings)
    {
        _cosmosClient = new CosmosClient(settings.ConnectionString);
        _databaseName = settings.DatabaseName;
        _containerId = typeof(T).Name;
    }

    private Task<Container> GetContainerAsync()
    {
        return _containerTask ??= CreateDatabaseAndContainerIfNotExistsAsync(_databaseName, _containerId, _partitionKeyPath);
    }

    private async Task<Container> CreateDatabaseAndContainerIfNotExistsAsync(string databaseId, string containerId, string partitionKeyPath)
    {
        var database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
        var containerResponse = await database.Database.CreateContainerIfNotExistsAsync(new ContainerProperties(containerId, partitionKeyPath));
        return containerResponse.Container;
    }

    public async Task<List<T>> Query(Expression<Func<T, bool>> predicate)
    {
        var container = await GetContainerAsync();
        var queryable = container.GetItemLinqQueryable<T>().Where(predicate).ToFeedIterator();
        var results = new List<T>();

        while (queryable.HasMoreResults)
        {
            var response = await queryable.ReadNextAsync();
            results.AddRange(response);
        }

        return results;
    }

    public virtual async Task<IEnumerable<T>> GetAll()
    {
        var container = await GetContainerAsync();
        var queryable = container.GetItemLinqQueryable<T>().ToFeedIterator();
        var results = new List<T>();

        while (queryable.HasMoreResults)
        {
            var response = await queryable.ReadNextAsync();
            results.AddRange(response);
        }

        return results;
    }

    public virtual async Task<T> GetById(Guid id, Guid pid)
    {
        var container = await GetContainerAsync();
        try
        {
            var response = await container.ReadItemAsync<T>(id.ToString(), new PartitionKey(pid.ToString()));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public virtual async Task Add(T entity)
    {
        var container = await GetContainerAsync();
        await container.CreateItemAsync<T>(entity, new PartitionKey(entity.Pid.ToString()));
    }

    public virtual async Task AddRange(IEnumerable<T> entities)
    {
        var container = await GetContainerAsync();
        foreach (var entity in entities)
        {
            await container.UpsertItemAsync<T>(entity, new PartitionKey(entity.Pid.ToString()));
        }
    }

    public virtual async Task CreateOrUpdate(T entity)
    {
        var container = await GetContainerAsync();
        try
        {
            await container.UpsertItemAsync<T>(entity, new PartitionKey(entity.Pid.ToString()));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public virtual async Task Update(T entity)
    {
        var container = await GetContainerAsync();
        await container.ReplaceItemAsync<T>(entity, entity.Id.ToString(), new PartitionKey(entity.Pid.ToString()));
    }

    public virtual async Task Delete(T entity)
    {
        var container = await GetContainerAsync();
        await container.DeleteItemAsync<T>(entity.Id.ToString(), new PartitionKey(entity.Pid.ToString()));
    }
}
