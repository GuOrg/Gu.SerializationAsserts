namespace Gu.SerializationAsserts
{
    using System;
    using System.Collections.Generic;

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

        internal static int IndexOf<TItem, TValue>(this IReadOnlyList<TItem> items, TItem item, Func<TItem, TValue> selector, IEqualityComparer<TValue> comparer)
        {
            for (int i = 0; i < items.Count; i++)
            {
                var x = selector(item);
                var y = selector(items[i]);
                if (comparer.Equals(x, y))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
