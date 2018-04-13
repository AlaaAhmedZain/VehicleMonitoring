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
    public interface IRepositoryAsyncExtension<T> where T : class
    {

        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] IncludeProperties);

        Task<IEnumerable<T>> ExecWithStoredProcedureAsync(string query, params object[] Parameters);

        Task<IEnumerable<T>> ExecWithStoredProcedureNonParametersAsync(string Query);

        Task<string> ExecScalarWithStoredProcedureAsync(string query, params object[] Parameters);

        Task<int> ExecWithStoredProcedureWithNoReturnAsync(string query, params object[] Parameters);

        Task<int> ExecStoredProcedureWithRowsAffectedAsync(string query, params object[] Parameters);

        Task<int> ExecuteSqlAsync(string Sql);
    }
}
