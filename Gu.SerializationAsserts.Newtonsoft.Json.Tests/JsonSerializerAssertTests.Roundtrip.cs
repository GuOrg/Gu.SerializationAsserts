namespace Gu.SerializationAsserts.Newtonsoft.Json.Tests
{
    using global::Newtonsoft.Json;
    using global::Newtonsoft.Json.Converters;

    using Gu.SerializationAsserts.Newtonsoft.Json.Tests.Dtos;

    using NUnit.Framework;

    public partial class JsonSerializerAssertTests
    {
        public class Roundtrip
        {
            private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                Converters = new[] { new StringEnumConverter() }
            };

            [Test]
            public void Dummy()
            {
                var actual = new Dummy { Value = 2 };
                var roundtrips = new[]
                                     {
                                         JsonSerializerAssert.Roundtrip(actual),
                                         JsonSerializerAssert.Roundtrip(actual, this.serializerSettings),
                                     };
                foreach (var roundtrip in roundtrips)
                {
                    Assert.AreNotSame(actual, roundtrip);
                    Assert.AreEqual(2, actual.Value);
                    Assert.AreEqual(2, roundtrip.Value);
                }
            }

            [Test]
            public void WithDummy()
            {
                var actual = new WithDummy { Dummy = new Dummy { Value = 2 } };
                var roundtrips = new[]
                                     {
                                         JsonSerializerAssert.Roundtrip(actual),
                                         JsonSerializerAssert.Roundtrip(actual, this.serializerSettings),
                                     };
                foreach (var roundtrip in roundtrips)
                {
                    Assert.AreNotSame(actual, roundtrip);
                    Assert.AreEqual(2, actual.Dummy.Value);
                    Assert.AreEqual(2, roundtrip.Dummy.Value);
                }
            }

            [Test]
            public void ArrayOfInts()
            {
                var actual = new[] { 1, 2, 3 };
                var roundtrips = new[]
                                     {
                                         JsonSerializerAssert.Roundtrip(actual),
                                         JsonSerializerAssert.Roundtrip(actual, this.serializerSettings),
                                     };
                foreach (var roundtrip in roundtrips)
                {
                    Assert.AreNotSame(actual, roundtrip);
                    CollectionAssert.AreEqual(new[] { 1, 2, 3 }, actual);
                    CollectionAssert.AreEqual(new[] { 1, 2, 3 }, roundtrip);
                }
            }

            [Test]
            public void ArrayOfDummies()
            {
                var actual = new[] { new Dummy(1), new Dummy(2), new Dummy(3), };
                var roundtrips = new[]
                                     {
                                         JsonSerializerAssert.Roundtrip(actual),
                                         JsonSerializerAssert.Roundtrip(actual, this.serializerSettings),
                                     };
                foreach (var roundtrip in roundtrips)
                {
                    Assert.AreNotSame(actual, roundtrip);
                    CollectionAssert.AreEqual(new[] { new Dummy(1), new Dummy(2), new Dummy(3), }, actual, Dtos.Dummy.Comparer);
                    CollectionAssert.AreEqual(new[] { new Dummy(1), new Dummy(2), new Dummy(3), }, roundtrip, Dtos.Dummy.Comparer);
                }
            }

            [Test]
            public void ThrowsWithWrongCtorParameter()
            {
                var actual = new WithWrongCtorParameter("meh");
                var ex = Assert.Throws<AssertException>(() => JsonSerializerAssert.Roundtrip(actual));
                var expectedMessage = "  Simple roundtrip failed. Source is not equal to roundtripped.\r\n" +
                                      "  AssertException:   Found this difference between expected and actual:\r\n" +
                                      "  expected.<Name>k__BackingField: meh\r\n" +
                                      "    actual.<Name>k__BackingField: null";
                Assert.AreEqual(expectedMessage, ex.Message);
            }
        }
    }
}