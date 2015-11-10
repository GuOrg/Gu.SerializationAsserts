using System;

namespace Gu.SerializationAsserts
{
    using System.CodeDom.Compiler;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    public static class BinaryAssert
    {
        /// <summary>
        /// Serializes <paramref name="expected"/> and <paramref name="actual"/> to memorystreams using <see cref="System.Runtime.Serialization.Formatters.Binary.BinaryFormatter"/>
        /// Then compares the bytes for <paramref name="expected"/> and <paramref name="actual"/>
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="expected"/> and <paramref name="actual"/></typeparam>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        public static void Equal<T>(T expected, T actual)
        {
            var expecteds = ToBytes(expected, nameof(expected));
            var actuals = ToBytes(actual, nameof(actual));
            Equal(expecteds, actuals);
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
            if (expected == null && actual == null)
            {
                return;
            }

            if (expected == null || actual == null)
            {
                throw new AssertException($"  Expected was: {expected?.ToString() ?? "null"}\r\n" +
                                          $"  Actual was: {actual?.ToString() ?? "null"}");
            }

            Equal(expected.ToArray(), actual.ToArray());
        }

        /// <summary>
        /// Compares two streams for equality.
        /// 1. Length
        /// 2. Contents
        /// </summary>
        /// <param name="expected">The expected bytes</param>
        /// <param name="actual">The actual bytes</param>
        public static void Equal(byte[] expected, byte[] actual)
        {
            if (expected == null && actual == null)
            {
                return;
            }

            if (expected == null || actual == null)
            {
                throw new AssertException($"  Expected was: {expected?.ToString() ?? "null"}\r\n" +
                                          $"  Actual was: {actual?.ToString() ?? "null"}");
            }

            if (expected.Length != actual.Length)
            {
                var message = $"  Expected bytes to have equal lengths.\r\n" +
                              $"  {nameof(expected)}: {expected.Length}.\r\n" +
                              $"  {nameof(actual)}:   {actual.Length}.";
                throw new AssertException(message);
            }

            for (int i = 0; i < expected.Length; i++)
            {
                if (expected[i] != actual[i])
                {
                    var message = $"  Expected bytes to be equal.\r\n" +
                                  $"  Bytes differ at index {i}.";
                    throw new AssertException(message);
                }
            }
        }

        // Using new here to hide it so it not called by mistake
        private new static void Equals(object x, object y)
        {
            throw new NotSupportedException($"{x}, {y}");
        }

        private static byte[] ToBytes(object o, string parameterName)
        {
            try
            {
                var formatter = new BinaryFormatter();
                using (var stream = new MemoryStream())
                {
                    formatter.Serialize(stream, o);
                    return stream.ToArray();
                }
            }
            catch (Exception e)
            {
                using (var writer = new IndentedTextWriter(new StringWriter(), "  "))
                {
                    writer.WriteLine($"  Writing {parameterName} to a stream failed.");
                    writer.Indent++;
                    writer.WriteMessages(e);
                    throw new AssertException(writer.InnerWriter.ToString(), e);
                }
            }
        }
    }
}