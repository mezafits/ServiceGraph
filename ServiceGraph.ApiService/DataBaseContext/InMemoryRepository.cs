
using System.Linq.Expressions;
using ServiceGraph.Common;
 
    public class InMemoryRepository<T> : ICosmosRepository<T> where T : BaseObject
    {
        private readonly Dictionary<(Guid Id, Guid Pid), T> _items;

        public InMemoryRepository()
        {
            _items = new Dictionary<(Guid, Guid), T>();
        }

        public Task<List<T>> Query(Expression<Func<T, bool>> predicate)
        {
            var query = _items.Values.AsQueryable().Where(predicate).ToList();
            return Task.FromResult(query);
        }

        public Task<IEnumerable<T>> GetAll()
        {
            var allItems = _items.Values.ToList();
            return Task.FromResult<IEnumerable<T>>(allItems);
        }

        public Task<T> GetById(Guid id, Guid pid)
        {
            if (_items.TryGetValue((id, pid), out T item))
            {
                return Task.FromResult(item);
            }

            return Task.FromResult<T>(null);
        }

        public Task Add(T entity)
        {
            if(!_items.ContainsKey((entity.Id, entity.Pid)))
            {
                _items.Add((entity.Id, entity.Pid), entity);
            }
            return Task.CompletedTask;
        }

        public Task AddRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                if (!_items.ContainsKey((entity.Id, entity.Pid)))
                {
                    _items.Add((entity.Id, entity.Pid), entity);
                }
            }

            return Task.CompletedTask;
        }

        public Task CreateOrUpdate(T entity)  
        {

            
            _items[(entity.Id, entity.Pid)] = entity;
            return Task.CompletedTask;
        }

        public Task Update(T entity)
        {
            _items[(entity.Id, entity.Pid)] = entity;
            return Task.CompletedTask;
        }

        public Task Delete(T entity)
        {
            _items.Remove((entity.Id, entity.Pid));
            return Task.CompletedTask;
        }
    }

 
