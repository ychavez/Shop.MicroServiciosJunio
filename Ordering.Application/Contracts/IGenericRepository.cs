using Ordering.Domain.Common;
using System.Linq.Expressions;

namespace Ordering.Application.Contracts
{
    public interface IGenericRepository<T> where T : EntityBase
    {
        Task<T> AddAsync(T entity);
        Task DeleteAsync(T entity);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeString = null);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
        Task<IReadOnlyList<T>> GetAsync(int offset, int limit, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params string[] includeStrings);
        Task<T> GetById(int Id);
        Task UpdateAsync(T entity);
    }
}