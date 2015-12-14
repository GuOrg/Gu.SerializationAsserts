namespace Gu.SerializationAsserts
{
    internal class ComparedPair
    {
        internal ComparedPair(object expected, object actual)
        {
            this.Expected = expected;
            this.Actual = actual;
        }

        internal object Expected { get; }

        internal object Actual { get; }

        internal bool IsEqual { get; set; }

        internal bool HasCompared(object expected, object actual)
        {
            return ReferenceEquals(expected, this.Expected) && 
                   ReferenceEquals(actual, this.Actual);
        }
    }
}