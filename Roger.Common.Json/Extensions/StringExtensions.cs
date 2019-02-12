using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Roger.Json.Extensions
{
    public static class StringExtensions
    {
        public static T Deserialize<T>(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}
