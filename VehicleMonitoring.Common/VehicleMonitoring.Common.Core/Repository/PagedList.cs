using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VehicleMonitoring.Common.Core.Repository
{
    [Serializable]
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">IQuerable passed via repository</param>
        /// <param name="pageIndex">Start Page index</param>
        /// <param name="pageSize">Maxiumum number of records per page</param>
        public PagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            int total = source.Count();
            this.TotalCount = total;
            this.TotalPages = total / pageSize;

            if (total % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;


            this.AddRange(source.Skip(pageIndex * pageSize).Take(pageSize).ToList());
        }


        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">List Data source, pagination will handled in memory for a given list</param>
        /// <param name="pageIndex">Start Page index</param>
        /// <param name="pageSize">Maxiumum number of records per page</param>
        public PagedList(IList<T> source, int pageIndex, int pageSize)
        {
            TotalCount = source.Count();
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.AddRange(source.Skip(pageIndex * pageSize).Take(pageSize).ToList());
        }

        /// <summary>
        /// Ctor // not recomended for large data queries
        /// </summary>
        /// <param name="source">IEnumerable data source, paginating query on the memeory </param>
        /// <param name="pageIndex">Start Page index</param>
        /// <param name="pageSize">Maxiumum number of records per page</param>
        /// <param name="totalCount">Total number of expected records</param>
        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            TotalCount = totalCount;
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.AddRange(source);
        }

        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }

        /// <summary>
        /// Previous Page Indicator
        /// </summary>
        public bool HasPreviousPage
        {
            get { return (PageIndex > 0); }
        }
        /// <summary>
        /// Next Page Indicator
        /// </summary>
        public bool HasNextPage
        {
            get { return (PageIndex + 1 < TotalPages); }
        }
    }
}
