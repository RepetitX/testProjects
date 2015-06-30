using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Core
{
    /// <summary>
    /// Static class containing String extension methods.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Case insensitive version of String.Replace().
        /// </summary>
        /// <param name="s">String that contains patterns to replace</param>
        /// <param name="oldValue">Pattern to find</param>
        /// <param name="newValue">New pattern to replaces old</param>
        /// <param name="comparisonType">String comparison type</param>
        /// <returns></returns>
        public static string Replace(this string s, string oldValue, string newValue,
            StringComparison comparisonType)
        {
            if (s == null)
                return null;

            if (String.IsNullOrEmpty(oldValue))
                return s;

            var result = new StringBuilder(Math.Min(4096, s.Length));
            var pos = 0;

            while (true)
            {
                int i = s.IndexOf(oldValue, pos, comparisonType);
                if (i < 0)
                    break;

                result.Append(s, pos, i - pos);
                result.Append(newValue);

                pos = i + oldValue.Length;
            }
            result.Append(s, pos, s.Length - pos);

            return result.ToString();
        }
    }
}
