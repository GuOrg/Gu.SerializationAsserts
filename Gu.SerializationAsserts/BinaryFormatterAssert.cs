namespace Gu.SerializationAsserts
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    /// <summary>
    /// Test serialization using <see cref="BinaryFormatter"/>
    /// </summary>
    public static class BinaryFormatterAssert
    {
        /// <summary>
        /// 1 Writes it to a memorystream using <see cref="BinaryFormatter"/>.
        /// 2 Deserializes it from the stream using <see cref="BinaryFormatter"/>.
        /// 3 Returns roundtripped instance
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="item>"/></typeparam>
        /// <param name="item">The item to serialize</param>
        /// <returns>The <typeparamref name="T"/> read from the stream</returns>
        public static T Roundtrip<T>(T item)
        {
            var firstBytes = BinaryAssert.ToBytes(item, typeof(T).Name);
            item = FromBytes<T>(firstBytes, typeof(T).Name);
            var secondBytes = BinaryAssert.ToBytes(item, typeof(T).Name);
            BinaryAssert.Equal(firstBytes, secondBytes);
            item = FromBytes<T>(firstBytes, typeof(T).Name);
            return item;
        }

        /// <summary>
        /// 1. Serializes <paramref name="expected"/> and <paramref name="actual"/> to bytes strings using <see cref="BinaryFormatter"/>
        /// 2. Compares the bytes using <see cref="BinaryAssert"/>
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="expected>"/> and <paramref name="actual"/></typeparam>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        public static void Equal<T>(T expected, T actual)
        {
            BinaryAssert.Equal(expected, actual);
        }

        /// <summary>
        /// Compares two streams for equality.
        /// 1. Length
        /// 2. Contents
        /// </summary>
        /// <param name="expected">The expected stream</param>
        /// <param name="actual">The actual stream</param>
        public static void Equal(MemoryStream expected, MemoryStream actual)
        {
            BinaryAssert.Equal(expected, actual);
        }

        private static T FromBytes<T>(byte[] bytes, string parameterName)
        {
            using (var stream = new MemoryStream(bytes))
            {
                try
                {
                    var binaryFormatter = new BinaryFormatter();
                    var value = (T)binaryFormatter.Deserialize(stream);
                    return value;
                }
                catch (Exception e)
                {
                    throw AssertException.CreateFromException($"Reading {parameterName} of type {typeof(T)} failed.", e);
                }
            }
        }

        // Using new here to hide it so it not called by mistake
        private static new void Equals(object x, object y)
        {
            throw new AssertException($"Don't call this {x}, {y}");
        }
    }
}