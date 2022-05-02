using Facilis.Core.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace Facilis.Core.Helpers
{
    public static class SortableHelper
    {
        public static IQueryable<T> Sort<T>(this IEnumerable<T> entities)
            where T : ISortableEntity
        {
            return entities
                .OrderBy(entity => entity.AscendingOrder)
                .AsQueryable();
        }

        public static IQueryable<T> ReverseSort<T>(this IEnumerable<T> entities)
            where T : ISortableEntity
        {
            return entities
                .OrderByDescending(entity => entity.AscendingOrder)
                .AsQueryable();
        }
    }
}