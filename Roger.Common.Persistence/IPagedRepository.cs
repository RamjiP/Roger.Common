using System.Threading.Tasks;

namespace Roger.Common.Persistence
{
    public interface IPagedRepository<T> : IRepository<T>
        where T : new()
    {
        Task<IPagedResult<T>> GetPagedResultAsync(string sql, SqlQueryOptions options);
    }
}