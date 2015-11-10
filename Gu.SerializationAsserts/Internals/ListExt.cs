namespace Gu.SerializationAsserts
{
    using System.Collections.Generic;
    using System.Linq;

    internal static class ListExt
    {
        internal static T ElementAtOrDefault<T>(this IReadOnlyList<T> items, int index, T @default = default(T))
        {
            if (items.Count > index)
            {
                return items[index];
            }

            return @default;
        }
    }
}
