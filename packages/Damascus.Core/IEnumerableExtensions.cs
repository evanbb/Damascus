using System.Collections.Generic;
using System.Linq;

namespace Damascus.Core
{
    public static class IEnumerableExtensions
    {
        public static bool NotEmpty<T>(this IEnumerable<T> enumerable)
        {
            return !(enumerable is null) && enumerable.Any();
        }
    }
}
