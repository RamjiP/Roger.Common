using System.Collections.Generic;

namespace Roger.Common.Persistence
{
    public interface ITokenPagedResult<T>
    {
        List<T> Data { get; set; }

        string Token { get; set; }
    }
}
