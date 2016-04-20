namespace Gu.SerializationAsserts
{
    /// <summary>Exposes factory methods for creating a <see cref="BinaryComparer{T}"/></summary>
    public static class BinaryComparer
    {
        /// <summary>Returns BinaryComparer&lt;T&gt;.Default;</summary>
        /// <returns>A <see cref="BinaryComparer{T}"/> for <paramref name="instance"/></returns>
        // ReSharper disable once UnusedParameter.Global
        public static BinaryComparer<T> For<T>(T instance)
        {
            return BinaryComparer<T>.Default;
        }
    }
}