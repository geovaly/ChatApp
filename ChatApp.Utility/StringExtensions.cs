using System;

namespace ChatApp.Utility
{
    public static class StringExtensions
    {
        public static string RemoveSubstring(this string s, string substring)
        {
            return s.Remove(s.IndexOf(substring, StringComparison.Ordinal), substring.Length);
        }
    }
}