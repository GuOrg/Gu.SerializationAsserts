using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

namespace Gu.SerializationAsserts
{
    using System;

    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    public static class BinaryFormatterAssert
    {
        /// <summary>
        /// 1 Writes it to a memorystream using <see cref="BinaryFormatter"/>.
        /// 2 Deserializes it from the stream using <see cref="BinaryFormatter"/>.
        /// 3 Returns roundtripped instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
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
                                              $"  {nameof(first)}:  { first.Length }.\r\n" +
                                              $"  {nameof(second)}: { second.Length}.";
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

        public static void Equals(MemoryStream expected, MemoryStream actual)
        {
            if (expected == null && actual == null)
            {
                return;
            }
            if (expected == null || actual == null)
            {
                throw new AssertException($"  Expected was: {expected?.ToString() ?? "null"}\r\n" +
                                          $"Actual was: {actual?.ToString() ?? "null"}");
            }
            if (expected.Length != actual.Length)
            {
                var message = $"  Expected stream lengthts to be equal.\r\n" +
                              $"  {nameof(expected)}: {expected.Length}.\r\n" +
                              $"  {nameof(actual)}:   {actual.Length}.";
                throw new AssertException(message);
            }
            var expecteds = expected.ToArray();
            var actuals = actual.ToArray();

            for (int i = 0; i < expecteds.Length; i++)
            {
                if (!Equals(expecteds[i], actuals[i]))
                {
                    var message = $"  Expected streams to be equal.\r\n" +
                                  $"  Streams differ at index {i}";
                    throw new AssertException(message);
                }
            }
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
    }
}