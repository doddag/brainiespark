using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace brainiespark.Helpers
{
    public static class Extensions
    {
        public static string[] ToArray(this string str, char[] delimiters)
        {
            if (!string.IsNullOrEmpty(str))
                return str.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            return new string[] {};
        }
    }
}