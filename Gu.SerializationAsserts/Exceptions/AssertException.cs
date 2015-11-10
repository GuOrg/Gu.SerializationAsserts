namespace Gu.SerializationAsserts
{
    using System;

    [Serializable]
    public class AssertException : Exception
    {
        public AssertException(string message)
            : base(message)
        {
        }

        public AssertException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}