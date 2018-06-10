using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Telegram.Net.SchemaGen
{
    public static class Extensions
    {
        public static string Capitalize(this string value)
        {
            if (value.Length == 0)
                return value;

            var chars = value.ToCharArray();
            chars[0] = char.ToUpperInvariant(chars[0]);

            return new string(chars);
        }

        public static IEnumerable<string> StringifyEnumerable<T>(this IEnumerable<T> value)
        {
            return value.Select(i => i.ToString());
        }

        public static (string, string) SplitToTuple(string value, char delim, bool defaultToFirstField)
        {
            var splitted = value.Split(delim);
            if (splitted.Length != 1 && splitted.Length != 2)
            {
                throw new ArgumentException();
            }

            if (splitted.Length == 2)
                return (splitted[0], splitted[1]);

            if (defaultToFirstField)
                return (splitted[0], null);

            return (null, splitted[0]);
        }

        public static string CompressSpaces(string value)
        {
            return Regex.Replace(value, "\\s+", " ");
        }
    }
}
