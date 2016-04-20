namespace Gu.SerializationAsserts
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>The results from a call to <see cref="SerializationAssert.RoundtripAll{T}"/></summary>
    public class RoundtripResults<T> : IEnumerable<T>
    {
        /// <summary> Initializes a new instance of the <see cref="RoundtripResults{T}"/> class.</summary>
        /// <param name="binaryFormatter">The result from the <see cref="BinaryFormatterAssert.Roundtrip{T}(T)"/></param>
        /// <param name="xmlSerializer">The result from the <see cref="XmlSerializerAssert.Roundtrip{T}(T)"/></param>
        /// <param name="dataContractSerializer">The result from the <see cref="DataContractSerializerAssert.Roundtrip{T}(T)"/></param>
        public RoundtripResults(T binaryFormatter, T xmlSerializer, T dataContractSerializer)
        {
            this.BinaryFormatter = binaryFormatter;
            this.XmlSerializer = xmlSerializer;
            this.DataContractSerializer = dataContractSerializer;
        }

        /// <summary>Gets the result from the <see cref="BinaryFormatterAssert.Roundtrip{T}(T)"/></summary>
        public T BinaryFormatter { get; }

        /// <summary>Gets the result from the <see cref="XmlSerializerAssert.Roundtrip{T}(T)"/></summary>
        public T XmlSerializer { get; }

        /// <summary>Gets the result from the <see cref="DataContractSerializerAssert.Roundtrip{T}(T)"/></summary>
        public T DataContractSerializer { get; }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            yield return this.BinaryFormatter;
            yield return this.XmlSerializer;
            yield return this.DataContractSerializer;
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}