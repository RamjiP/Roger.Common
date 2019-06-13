using System.Collections.Generic;

namespace Roger.Common.Persistence
{
    public class PagedResult<T> : IPagedResult<T>
    {
        public int TotalRowCount { get; set; }
        public List<T> Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool HasNextPage { get; set; }

        public PagedResult()
        {
            Data = new List<T>();
        }
    }
}