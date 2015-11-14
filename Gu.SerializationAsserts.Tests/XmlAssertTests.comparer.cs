namespace Gu.SerializationAsserts.Tests
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Xml.Linq;

    using NUnit.Framework;

    public partial class XmlAssertTests
    {
        [Test]
        public void EqualWithCustomElementComparer()
        {
            var expected = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                           "<Foo xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                           "  <Bar>1</Bar>" +
                           "</Foo>";

            var actual = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                         "<Foo xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                         "  <Bar>  1.0  </Bar>" +
                         "</Foo>";
            XmlAssert.Equal(expected, actual, new ElementDoubleValueComparer(), null);
        }

        private class ElementDoubleValueComparer : IEqualityComparer<XElement>
        {
            public bool Equals(XElement x, XElement y)
            {
                return double.Parse(x.Value, CultureInfo.InvariantCulture) == double.Parse(y.Value, CultureInfo.InvariantCulture);
            }

            public int GetHashCode(XElement obj)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
