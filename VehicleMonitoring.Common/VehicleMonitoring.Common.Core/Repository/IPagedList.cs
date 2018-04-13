using System;
using System.Collections.Generic;
using System.Text;

namespace VehicleMonitoring.Common.Core.Repository
{
    /// <summary>
    /// Basic Implementation for Pagination 
    /// </summary>
    /// <typeparam name="T">Domain Model to be paginated</typeparam>
    internal interface IPagedList<T> : IList<T>
    {
        int PageIndex { get; }
        int PageSize { get; }
        int TotalCount { get; }
        int TotalPages { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }


    }
}
