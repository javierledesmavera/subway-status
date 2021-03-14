using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Subway.Status.Repository.Contracts
{
    public interface IRepository<TEntity>
        where TEntity : class, new()
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<TEntity> DeleteAsync(TEntity entity);
    }
}
