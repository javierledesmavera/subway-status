using Subway.Status.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Subway.Status.Repository
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity: class, new()
    {
        protected readonly SubwayContext SubwayContext;

        public Repository(SubwayContext subwayContext)
        {
            SubwayContext = subwayContext;
        }

        public IQueryable<TEntity> GetAll()
        {
            try
            {
                return SubwayContext.Set<TEntity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"No se pudieron obtener las entidades: {ex.Message}");
            }
        }

        public IQueryable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return SubwayContext.Set<TEntity>().Where(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception($"No se pudieron obtener las entidades: {ex.Message}");
            }
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"La entidad {nameof(entity)} no debe ser nula.");
            }

            try
            {
                await SubwayContext.AddAsync(entity);
                await SubwayContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"La entidad {nameof(entity)} no pudo ser guardada: {ex.Message}");
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"La entidad {nameof(entity)} no debe ser nula.");
            }

            try
            {
                SubwayContext.Update(entity);
                await SubwayContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"La entidad {nameof(entity)} no pudo ser actualizada: {ex.Message}");
            }
        }

        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"La entidad {nameof(entity)} no debe ser nula.");
            }

            try
            {
                SubwayContext.Remove(entity);
                await SubwayContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"La entidad {nameof(entity)} no pudo ser eliminada: {ex.Message}");
            }
        }
    }
}
