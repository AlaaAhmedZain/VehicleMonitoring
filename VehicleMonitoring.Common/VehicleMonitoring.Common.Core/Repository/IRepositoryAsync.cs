using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace VehicleMonitoring.Common.Core.Repository
{
    /// <summary>
    /// Based on the Interface segregation Princeple(ISP), Separating regular Repository from the Async Repository 
    /// that is not to force all clients to implement Async behavior unless it is needed
    /// </summary>
    /// <typeparam name="T">Domain Entity</typeparam>
    public interface IRepositoryAsync<T> where T : class
    {
        Task<T> AddAsync(T Entity);

        Task<IEnumerable<T>> AddAsync(IEnumerable<T> entities);

        Task<T> UpdateAsync(T Entity);

        Task<IEnumerable<T>> UpdateAsync(IEnumerable<T> entities);

        Task<int> DeleteAsync(T Entity);

        Task<int> DeleteAsync(object KeyValue);

        Task<int> DeleteAsync(Expression<Func<T, bool>> Expression);

        Task<T> FindByIdAsync(object KeyValue);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> Expression);

        Task<IEnumerable<T>> AllAsync(bool AsNoTracking = true);

        Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> Expression);

        Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> Predicate, Expression<Func<T, string>> Order);

        /// <summary>
        /// For Easy pagination at the DB level
        /// </summary>
        /// <param name="query">Query for data filter</param>
        /// <param name="pageIndex">the index of the page </param>
        /// <param name="pageSize">Page size represent the maximum number of records retrived per page</param>
        /// <returns>Paged List</returns>
        Task<PagedList<T>> PaginateAsync(IQueryable<T> query, int pageIndex = 0, int pageSize = int.MaxValue);

        Task<IEnumerable<T>> GetAsync(IQueryable<T> Queryable);

        Task<IEnumerable<T>> SelectAsync(System.Linq.Expressions.Expression<Func<T, int, T>> selector);

        Task<IEnumerable<T>> SelectManyAsync(System.Linq.Expressions.Expression<Func<T, IEnumerable<T>>> selector);

        Task<int> CountAsync();

        Task<int> CountAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate);

        Task<long> LongCountAsync();

        Task<long> LongCountAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate);

    }
}
