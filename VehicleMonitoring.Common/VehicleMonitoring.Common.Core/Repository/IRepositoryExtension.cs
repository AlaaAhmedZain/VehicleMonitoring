using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace VehicleMonitoring.Common.Core.Repository
{
    /// <summary>
    /// Based on Open Close Princeple (OCP) and Interface Segregration Principle (ISP) separating 
    /// the Relational Database repository functionalities, from the Generic Data store
    ///</summary>
    /// <typeparam name="T">Domain Entity</typeparam>
    public interface IRepositoryExtension<T> where T : class
    {

        IEnumerable<T> All(bool AsNoTracking = true);

        IQueryable<T> AsNoTracking();

        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] IncludeProperties);

        IEnumerable<T> ExecWithStoredProcedure(string query, params object[] Parameters);

        IEnumerable<T> ExecWithStoredProcedureNonParameters(string Query);

        string ExecScalarWithStoredProcedure(string query, params object[] Parameters);

        void ExecWithStoredProcedureWithNoReturn(string query, params object[] Parameters);

        int ExecStoredProcedureWithRowsAffected(string query, params object[] Parameters);

        int ExecuteSql(string Sql);
    }
}
