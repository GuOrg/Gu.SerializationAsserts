namespace Gu.SerializationAsserts.Newtonsoft.Json.Tests
{
    using System;
    using System.Collections.Generic;
    using global::Newtonsoft.Json.Linq;
    using NUnit.Framework;

    public partial class JsonAssertTests
    {
        [Test]
        public void EqualWithCustomElementComparer()
        {
            var expected = "{\"Value\":1.234}";
            var actual = "{\"Value\":1.2345}";
            JsonAssert.Equal(expected, actual, new DoubleValueComparer());
        }

        private class DoubleValueComparer : IEqualityComparer<JValue>
        {
            public bool Equals(JValue x, JValue y)
            {
                var xv = (double)x.Value;
                var yv = (double)y.Value;
                return Math.Abs(xv - yv) < 1E-3;
            }

            int IEqualityComparer<JValue>.GetHashCode(JValue obj)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
