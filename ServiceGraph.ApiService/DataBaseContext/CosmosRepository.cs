
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.Linq.Expressions;
using System.Net;
using ServiceGraph.Common;


public class CosmosRepository<T> : ICosmosRepository<T> where T : BaseObject
{
    private readonly CosmosClient _cosmosClient;
    private readonly Container _container;

    public CosmosRepository(DatabaseSettings settings)
    {
        _cosmosClient = new CosmosClient(settings.ConnectionString);
        _container = CreateDatabaseAndContainerIfNotExistsAsync(settings.DatabaseName, typeof(T).Name, @"/pid").Result;

    }
    private async Task<Container> CreateDatabaseAndContainerIfNotExistsAsync(string databaseId, string containerId, string partitionKeyPath)
    {
        var database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
        var containerResponse = await database.Database.CreateContainerIfNotExistsAsync(new ContainerProperties(containerId, partitionKeyPath));
        return containerResponse.Container;
    }
    public async Task<List<T>> Query(Expression<Func<T, bool>> predicate)
    {
        var queryable = _container.GetItemLinqQueryable<T>().Where(predicate).ToFeedIterator();
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
        var queryable = _container.GetItemLinqQueryable<T>().ToFeedIterator();
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
        try
        {

            var response = await _container.ReadItemAsync<T>(id.ToString(), new PartitionKey(pid.ToString()));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public virtual async Task Add(T entity)
    {
        var itemResponse = await _container.CreateItemAsync<T>(entity, new PartitionKey(entity.Pid.ToString()));
    }

    public virtual async Task AddRange(IEnumerable<T> entity)
    {
        var items = entity.ToArray();
        for (int i = 0; i < items.Length; i++)
        {
            await _container.UpsertItemAsync<T>(items[i], new PartitionKey(items[i].Pid.ToString()));
            //await _container.CreateItemAsync<T>(items[i], new PartitionKey(items[i].Pid));
        }
    }
    public virtual async Task CreateOrUpdate(T entity)
    {
        try
        {
            var itemResponse = await _container.UpsertItemAsync<T>(entity, new PartitionKey(entity.Pid.ToString()));
        }
        catch (Exception err)
        {
            Console.Write(err.Message);
            throw;
        }
    }


    public virtual async Task Update(T entity)
    {
        await _container.ReplaceItemAsync<T>(entity, entity.Id.ToString(), new PartitionKey(entity.Pid.ToString()));
    }

    public virtual async Task Delete(T entity)
    {
        await _container.DeleteItemAsync<T>(entity.Id.ToString(), new PartitionKey(entity.Pid.ToString()));
    }
}