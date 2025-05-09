using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ServiceGraph.Common;

public interface ICosmosRepository<T> where T : BaseObject
    {
        Task<List<T>> Query(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(Guid id,Guid pid);
        Task Add(T entity);
        Task CreateOrUpdate(T entity);
        Task AddRange(IEnumerable<T> entities);    
        Task Update(T entity);
        Task Delete(T entity);
    }
 

