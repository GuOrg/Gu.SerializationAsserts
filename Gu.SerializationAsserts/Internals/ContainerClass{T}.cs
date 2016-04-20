namespace Gu.SerializationAsserts
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// This class is used in serialization roundtrips.
    /// The purpose is for catching errors with ReadEndElement etc.
    /// </summary>
    [Serializable]
    [DataContract(Name = XmlName)]
    [XmlRoot(XmlName)]
    public class ContainerClass<T>
    {
        internal const string XmlName = "ContainerClass";

        public ContainerClass(T item)
        {
            this.First = item;
            this.Other = item;
        }

        private ContainerClass()
        {
        }

        [DataMember(Name = nameof(First))]
        public T First { get; set; }

        [DataMember(Name = nameof(Other))]
        public T Other { get; set; }
    }
}