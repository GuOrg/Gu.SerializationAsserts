namespace Gu.SerializationAsserts
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Xml.Linq;

    /// <inheritdoc/>
    public class XAttributeDoubleValueComparer : IEqualityComparer<XAttribute>
    {
        /// <summary>The default instance.</summary>
        public static readonly XAttributeDoubleValueComparer Default = new XAttributeDoubleValueComparer(0);
        private readonly double tolerance;

        /// <summary>Initializes a new instance of the <see cref="XAttributeDoubleValueComparer"/> class.</summary>
        /// <param name="tolerance">The tolerance when comparing numeric values.</param>
        public XAttributeDoubleValueComparer(double tolerance)
        {
            this.tolerance = tolerance;
        }

        /// <summary>return Math.Abs(xValue - yValue) &lt;= this.tolerance;</summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        /// <returns>true if the trimmed values are equal</returns>
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