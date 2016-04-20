namespace Gu.SerializationAsserts
{
    /// <summary>Factor class for <seealso cref="FieldComparer{T}"/> </summary>
    public static class FieldComparer
    {
        /// <summary>
        /// Returns an <see cref="FieldComparer{T}.Default"/> for <paramref name="instance"/>
        /// </summary>
        /// <returns>The <see cref="FieldComparer{T}.Default"/>.</returns>
        // ReSharper disable once UnusedParameter.Global
        public static FieldComparer<T> For<T>(T instance)
        {
            return FieldComparer<T>.Default;
        }
    }
}