using System.Threading.Tasks;

namespace Roger.Common.Persistence
{
    public interface ITokenRepository<T> : IRepository<T>
        where T : new()
    {
        Task<ITokenPagedResult<T>> GetTokenisedResultAsync(SqlQueryOptions sqlQueryOptions);
    }
}
