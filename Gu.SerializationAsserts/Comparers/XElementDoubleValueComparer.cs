namespace Gu.SerializationAsserts
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Xml.Linq;

    public class XElementDoubleValueComparer : IEqualityComparer<XElement>
    {
        private readonly double tolerance;
        public static readonly XElementDoubleValueComparer Default = new XElementDoubleValueComparer(0);

        public XElementDoubleValueComparer(double tolerance)
        {
            this.tolerance = tolerance;
        }

        public bool Equals(XElement x, XElement y)
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

        int IEqualityComparer<XElement>.GetHashCode(XElement obj)
        {
            throw new NotSupportedException();
        }
    }
}