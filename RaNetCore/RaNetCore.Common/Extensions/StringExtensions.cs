using System;
using System.Linq;

namespace RaNetCore.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToPascalCase(this string str)
        {
            if (str.Length == 0)
                return string.Empty;
            else if (str.Length == 1)
                return char.ToUpper(str[0]).ToString();
            else
                return $"{char.ToUpper(str[0])}{str.Substring(1)}";
        }

        public static string SeparateWords(this string str, string separator = " ")
        {
            return string.Concat(str
                .Select(x => Char.IsUpper(x)
                    ? separator + x
                    : x.ToString()
                )
            )
            .TrimStart(' ');
        }
    }
}
