namespace Gu.SerializationAsserts
{
    using System;
    using System.CodeDom.Compiler;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    public static class BinaryFormatterAssert
    {
        /// <summary>
        /// 1 Writes it to a memorystream using <see cref="BinaryFormatter"/>.
        /// 2 Deserializes it from the stream using <see cref="BinaryFormatter"/>.
        /// 3 Returns roundtripped instance
        /// </summary>
        /// <param name="item">The item to serialize</param>
        /// <returns>The <see cref="T"/> read from the stream</returns>
        public static T Roundtrip<T>(T item)
        {
            var bytes = new byte[2][];
            for (int i = 0; i < 2; i++)
            {
                using (var stream = new MemoryStream())
                {
                    try
                    {
                        ToStream(item, stream);
                    }
                    catch (Exception e)
                    {
                        using (var writer = new IndentedTextWriter(new StringWriter(), "  "))
                        {
                            writer.WriteLine($"  Writing {typeof(T).Name} to a stream failed.");
                            writer.Indent++;
                            writer.WriteMessages(e);
                            throw new AssertException(writer.InnerWriter.ToString(), e);
                        }
                    }

                    try
                    {
                        bytes[i] = stream.ToArray();

                        var fromStream = FromStream<T>(stream);
                        if (i == 1)
                        {
                            var first = bytes[0];
                            var second = bytes[1];
                            if (first.Length != second.Length)
                            {
                                var message = $"  Expected stream lengthts to be same.\r\n" +
                                              $"  {nameof(first)}:  {first.Length}.\r\n" +
                                              $"  {nameof(second)}: {second.Length}.";
                                throw new AssertException(message);
                            }

                            for (int j = 0; j < first.Length; j++)
                            {
                                if (first[j] != second[j])
                                {
                                    {
                                        var message = $"  Expected streams to be equal both times.\r\n" +
                                                      $"  Streams differ at index {j}";
                                        throw new AssertException(message);
                                    }
                                }
                            }

                            return fromStream;
                        }
                    }
                    catch (Exception e)
                    {
                        using (var writer = new StringWriter())
                        {
                            writer.WriteLine($" Reading {typeof(T).Name} to a stream failed");
                            writer.WriteMessages(e);
                            throw new AssertException(writer.ToString(), e);
                        }
                    }
                }
            }

            throw new InvalidOperationException("Mega derp happend");
        }

        /// <summary>
        /// 1. Serializes <paramref name="expected"/> and <paramref name="actual"/> to bytes strings using <see cref="BinaryFormatter"/>
        /// 2. Compares the bytes using <see cref="BinaryAssert"/>
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
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

        private static void ToStream<T>(T item, Stream stream)
        {
            var binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(stream, item);
            stream.Position = 0;
        }

        private static T FromStream<T>(MemoryStream stream)
        {
            var binaryFormatter = new BinaryFormatter();
            var value = (T)binaryFormatter.Deserialize(stream);
            return value;
        }

        // Using new here to hide it so it not called by mistake
        private new static void Equals(object x, object y)
        {
            throw new NotSupportedException($"{x}, {y}");
        }
    }
}