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

        internal static void AddIfNotExists<T>(this List<T> list, T item)
        {
            Ensure.NotNull(list, nameof(list));
            if (typeof(T).IsValueType)
            {
                if (list.All(x => !Equals(x, item)))
                {
                    list.Add(item);
                }
            }

            if (list.All(x => !ReferenceEquals(x, item)))
            {
                list.Add(item);
            }
        }
    }
}
