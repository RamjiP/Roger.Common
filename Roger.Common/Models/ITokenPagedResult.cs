using System.Collections.Generic;

namespace Roger.Common.Models
{
    public interface ITokenPagedResult<T>
    {
        List<T> Data { get; set; }

        string Token { get; set; }
    }
}
