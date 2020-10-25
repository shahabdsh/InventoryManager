using System;
using System.Linq;

namespace InventoryManager.Api
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string str1, string str2)
        {
            return str1.Equals(str2, StringComparison.OrdinalIgnoreCase);
        }

        public static bool ContainsIgnoreCase(this string str1, string str2)
        {
            return str1.ToLower().Contains(str2.ToLower());
        }
    }
}
