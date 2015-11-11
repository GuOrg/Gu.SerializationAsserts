namespace Gu.SerializationAsserts
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// This is for catching errors with ReadEndElement etc.
    /// </summary>
    /// <typeparam name="T">The type to contain</typeparam>
    [Serializable]
    [DataContract(Name = XmlName)]
    [XmlRoot(XmlName)]
    public class ContainerClass<T>
    {
        public const string XmlName = "ContainerClass";

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