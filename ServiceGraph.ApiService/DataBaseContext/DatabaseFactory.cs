
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ServiceGraph.Common;
 
    public class RepositoryFactory
    {
        private readonly DatabaseSettings _config;
        private readonly object _myLock = new object();
        private readonly Dictionary<string, dynamic> repositories = new Dictionary<string, dynamic>();

        public RepositoryFactory(DatabaseSettings configuration)
        {
            _config = configuration;
        }

        public RepositoryFactory(IOptions<DatabaseSettings> configuration)
        {
            _config = configuration.Value;
        }

        public ICosmosRepository<T> CreateRepository<T>() where T : BaseObject
        {
            lock (_myLock)
            {
                string typeName = typeof(T).Name;

                if (repositories.ContainsKey(typeName))
                {
                    return (ICosmosRepository<T>)repositories[typeName];
                }
                else
                {
                    ICosmosRepository<T> repo;

                    if (_config.UseInMemoryDatabase)
                    {
                        repo = new InMemoryRepository<T>();
                    }
                    else
                    {
                        repo = new CosmosRepository<T>(_config);
                    }

                    repositories.Add(typeName, repo);
                    return repo;
                }
            }
        }
    }


