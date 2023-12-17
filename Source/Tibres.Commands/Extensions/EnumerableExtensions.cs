using System.Collections.Generic;
using System.Linq;

namespace Tibres.Commands
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<(T Item, int Index)> WithIndex<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Select((item, index) => (item, index));
        }
    }
}
