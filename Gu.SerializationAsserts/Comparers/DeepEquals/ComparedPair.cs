namespace Gu.SerializationAsserts
{
    internal class ComparedPair
    {
        internal object First;
        internal object Other;

        internal ComparedPair(object first, object other)
        {
            this.First = first;
            this.Other = other;
        }

        internal bool IsEqual { get; set; }

        internal bool HasCompared(object first, object other)
        {
            return ReferenceEquals(first, this.First) && ReferenceEquals(other, this.Other);
        }
    }
}