namespace Gu.SerializationAsserts
{
    public static class BinaryAssert
    {
        /// <summary>
        /// Serializes <paramref name="x"/> and <paramref name="y"/> to memorystreams using <see cref="System.Runtime.Serialization.Formatters.Binary.BinaryFormatter"/>
        /// Then compares the bytes for <paramref name="x"/> and <paramref name="y"/>
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="x"/> and <paramref name="y"/></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>True if the serialized bytes matches</returns>
        public static bool Equals<T>(T x, T y)
        {
            return BinaryEqualsComparer<T>.Default.Equals(x, y);
        }
    }
}