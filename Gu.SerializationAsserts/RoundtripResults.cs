namespace Gu.SerializationAsserts
{
    using System.Collections;
    using System.Collections.Generic;

    public class RoundtripResults<T> : IEnumerable<T>
    {
        public RoundtripResults(T binaryFormatter, T xmlSerializer, T dataContractSerializer)
        {
            this.BinaryFormatter = binaryFormatter;
            this.XmlSerializer = xmlSerializer;
            this.DataContractSerializer = dataContractSerializer;
        }

        public T BinaryFormatter { get;  }

        public T XmlSerializer { get; }

        public T DataContractSerializer { get; }

        public IEnumerator<T> GetEnumerator()
        {
            yield return this.BinaryFormatter;
            yield return this.XmlSerializer;
            yield return this.DataContractSerializer;
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}