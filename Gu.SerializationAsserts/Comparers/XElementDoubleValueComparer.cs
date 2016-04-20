namespace Gu.SerializationAsserts
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Xml.Linq;

    /// <summary>A comparer for <see cref="XElement"/> with Value that can be parsed to a <see cref="double"/></summary>
    public class XElementDoubleValueComparer : IEqualityComparer<XElement>
    {
        /// <summary>The default instance with tolerance == 0.</summary>
        public static readonly XElementDoubleValueComparer Default = new XElementDoubleValueComparer(0);
        private readonly double tolerance;

        /// <summary>Initializes a new instance of the <see cref="XElementDoubleValueComparer"/> class.</summary>
        /// <param name="tolerance">The tolerance when comparing numeric values.</param>
        public XElementDoubleValueComparer(double tolerance)
        {
            this.tolerance = tolerance;
        }

        /// <summary>return Math.Abs(xValue - yValue) &lt;= this.tolerance;</summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        /// <returns>true if the trimmed values are equal</returns>
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