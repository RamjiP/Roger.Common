using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roger.Common.Persistence.Extensions
{

    public static class StringCollectionExtensions
    {
        public static string JoinWithComma(this List<string> values)
        {
            return string.Join(',', values.Select(value => $"'{value}'"));
        }
    }
}
