namespace Gu.SerializationAsserts.Tests.Dtos
{
    using System.Runtime.Serialization;

    [DataContract(Name = nameof(DataContractDummy))]
    public class DataContractDummy
    {
        [DataMember]
        public int Value { get; set; }
    }
}
