namespace Gu.SerializationAsserts.Newtonsoft.Json.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using global::Newtonsoft.Json.Linq;
    using NUnit.Framework;

    public partial class JsonAssertTests
    {
        [Test]
        public void EqualWithCustomElementComparer()
        {
            var expected = "<?Json version=\"1.0\" encoding=\"utf-16\"?>" +
                           "<Foo Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">" +
                           "  <Bar>1</Bar>" +
                           "</Foo>";

            var actual = "<?Json version=\"1.0\" encoding=\"utf-16\"?>" +
                         "<Foo Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">" +
                         "  <Bar>  1.0  </Bar>" +
                         "</Foo>";
            Assert.Fail();
            //JsonAssert.Equal(expected, actual, new DoubleValueComparer(), null);
        }

        private class DoubleValueComparer : IEqualityComparer<JProperty>
        {
            public bool Equals(JProperty x, JProperty y)
            {
                throw new NotImplementedException();
                //return double.Parse(x.Value, CultureInfo.InvariantCulture) == double.Parse(y.Value, CultureInfo.InvariantCulture);
            }

            public int GetHashCode(JProperty obj)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
