using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts;
using Ordering.Domain.Common;
using Ordering.Infraestructure.Persistence;
using System.Linq.Expressions;

namespace Ordering.Infraestructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : EntityBase
    {
        private readonly OrderContext orderContext;

        public GenericRepository(OrderContext orderContext)
        {
            this.orderContext = orderContext;
        }

        /// <summary>
        /// Nos trae toda la tabla completa
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyList<T>> GetAllAsync()
            => await orderContext.Set<T>().ToListAsync();

        /// <summary>
        /// nos trae todos los resultados que coincidan
        /// con la expresion predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
            => await orderContext.Set<T>().Where(predicate).ToListAsync();

        /// <summary>
        /// nos permite ejecutar una consulta haciendo uso de ordenamiento, filtrado e include (Join)
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeString"></param>
        /// <returns></returns>
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeString = null)
        {

            IQueryable<T> query = orderContext.Set<T>();

            if (!string.IsNullOrEmpty(includeString)) query = query.Include(includeString);

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null)
                return await orderBy(query).ToListAsync();


            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(int offset, int limit,
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params string[] includeStrings)
        {
            IQueryable<T> query = orderContext.Set<T>();

            query = query.Skip(offset).Take(limit);


            foreach (var itemInclude in includeStrings)
            {
                query = query.Include(itemInclude);
            }

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null)
                return await orderBy(query).ToListAsync();

            return await query.ToListAsync();

        }

        public async Task<T> GetById(int Id)
            => await orderContext.Set<T>().SingleAsync(x => x.Id == Id);

        public async Task<T> AddAsync(T entity)
        {
            await orderContext.Set<T>().AddAsync(entity);
            await orderContext.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            orderContext.Entry(entity).State = EntityState.Modified;
            await orderContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            orderContext.Set<T>().Remove(entity);
            await orderContext.SaveChangesAsync();
        }

    }
}
