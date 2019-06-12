using System;
using System.Collections.Generic;
using System.Text;

namespace Roger.Common.Extensions
{
    public static class StringExtensions
    {
        public static byte[] GetUtf8Bytes(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }
    }
}
