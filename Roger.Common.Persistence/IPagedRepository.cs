using System.Threading.Tasks;

namespace Roger.Common.Persistence
{
    public interface IPagedRepository<T> : IRepository<T>
        where T : new()
    {
        Task<IPagedResult<T>> GetAllAsync(string sqlQuery, int pageNumber, int pageSize);
    }
}