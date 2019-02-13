using System.Threading.Tasks;

namespace Roger.Common.Persistence
{
    public interface ITokenRepository<T>
        where T : new()
    {
        Task<T> CreateAsync(T doc);
        Task<T> CreateOrUpdateAsync(T doc);
        Task<T> UpdateAsync(string id, T doc);
        Task DeleteAsync(string id);
        Task<T> GetByIdAsync(string id, bool throwException = true);
        Task<ITokenPagedResult<T>> GetAllAsync(int maxItemCount = 10, string continuationToken = null);
    }
}
