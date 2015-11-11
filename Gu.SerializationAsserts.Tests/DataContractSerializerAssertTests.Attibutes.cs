namespace Gu.SerializationAsserts.Tests
{
    using Gu.SerializationAsserts.Tests.Dtos;

    using NUnit.Framework;

    public partial class DataContractSerializerAssertTests
    {
        [Test]
        public void HasDataContractAttributeHappyPath()
        {
            DataContractSerializerAssert.HasDataContractAttribute<DataContractDummy>();
            DataContractSerializerAssert.HasDataContractAttribute(typeof(DataContractDummy));
        }

        [Test]
        public void HasDataContractAttributeThrowsIfMissing()
        {
            var expected = "  Expected type Gu.SerializationAsserts.Tests.Dtos.MissingAttributes to have [DataContractAttribute]";

            var ex1 = Assert.Throws<AssertException>(() => DataContractSerializerAssert.HasDataContractAttribute<MissingAttributes>());
            Assert.AreEqual(expected, ex1.Message);

            var ex2 = Assert.Throws<AssertException>(() => DataContractSerializerAssert.HasDataContractAttribute(typeof(MissingAttributes)));
            Assert.AreEqual(expected, ex2.Message);
        }

        [Test]
        public void AllPropertiesHasDataMemberAttributesHappyPath()
        {
            DataContractSerializerAssert.AllPropertiesHasDataMemberAttributes<DataContractDummy>();
            DataContractSerializerAssert.AllPropertiesHasDataMemberAttributes(typeof(DataContractDummy));
        }

        [Test]
        public void AllPropertiesHasDataMemberAttributesThrowsIfMissing()
        {
            var expected = "  Expected all properties for type Gu.SerializationAsserts.Tests.Dtos.MissingAttributes to have [DataMemberAttribute]\r\n" +
                           "  The following properties does not:\r\n" +
                           "    Value";

            var ex1 = Assert.Throws<AssertException>(() => DataContractSerializerAssert.AllPropertiesHasDataMemberAttributes<MissingAttributes>());
            Assert.AreEqual(expected, ex1.Message);

            var ex2 = Assert.Throws<AssertException>(() => DataContractSerializerAssert.AllPropertiesHasDataMemberAttributes(typeof(MissingAttributes)));
            Assert.AreEqual(expected, ex2.Message);
        }
    }
}
