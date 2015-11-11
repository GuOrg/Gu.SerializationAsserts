namespace Gu.SerializationAsserts
{
    using System.Diagnostics;

    [DebuggerDisplay("Index: {Index} Value: {Value}")]
    internal class ComparedItem : ICompared
    {
        public ComparedItem(object value, int index)
        {
            this.Value = value;
            this.Index = index;
        }

        public object Value { get; }

        public int Index { get; }
    }
}
