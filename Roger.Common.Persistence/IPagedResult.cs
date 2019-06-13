using System.Collections.Generic;

namespace Roger.Common.Persistence
{
    public interface IPagedResult<T>
    {
        int TotalRowCount { get; set; }
        List<T> Data { get; set; }
        int PageNumber { get; set; }
        int PageSize { get; set; }
        bool HasNextPage { get; set; }
    }
}