namespace Gu.SerializationAsserts
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Xml.Linq;

    public class XAttributeDoubleValueComparer : IEqualityComparer<XAttribute>
    {
        private readonly double tolerance;
        public static readonly XAttributeDoubleValueComparer Default = new XAttributeDoubleValueComparer(0);

        public XAttributeDoubleValueComparer(double tolerance)
        {
            this.tolerance = tolerance;
        }

        public bool Equals(XAttribute x, XAttribute y)
        {
            if (x.Value == y.Value)
            {
                return true;
            }

            double xValue;
            double yValue;
            if (double.TryParse(x.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out xValue) &&
                double.TryParse(y.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out yValue))
            {
                return Math.Abs(xValue - yValue) <= this.tolerance;
            }

            return false;
        }

        int IEqualityComparer<XAttribute>.GetHashCode(XAttribute obj)
        {
            throw new NotSupportedException();
        }
    }
}