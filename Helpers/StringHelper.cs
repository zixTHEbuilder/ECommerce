using Microsoft.Extensions.Primitives;
using System.Diagnostics.Eventing.Reader;

namespace ECommerce.Helpers
{
    public class StringHelper
    {
        public static bool NullOrEmptyChecker(params string[] values)
        {
            return values.Any(string.IsNullOrEmpty);
        }
        public string? TrimString(string text)
        {
            if (text is null) return null;
            return text.Trim();
        }
        public string? NullCheckerAndTrim(string text)
        {
            if (NullOrEmptyChecker(text) is false) return null;
            return text.Trim();
        }
    }
}
