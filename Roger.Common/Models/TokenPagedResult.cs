using System.Collections.Generic;

namespace Roger.Common.Models
{
    public class TokenPagedResult<T> : ITokenPagedResult<T>
    {
        public List<T> Data { get; set; }
        public string Token { get; set; }

        public TokenPagedResult()
        {
            Data = new List<T>();
        }
    }
}