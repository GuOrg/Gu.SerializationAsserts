namespace Gu.SerializationAsserts
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Needed to break recursion
    /// </summary>
    internal class ComparisonTracker
    {
        private readonly ConditionalWeakTable<object, ICompared> map = new ConditionalWeakTable<object, ICompared>();

        internal bool HasCompared(object item)
        {
            if (item == null || item.GetType().IsValueType)
            {
                return false;
            }

            ICompared temp;
            return this.map.TryGetValue(item, out temp);
        }

        internal ICompared GetOrAdd(object item, Func<object, ICompared> creator)
        {
            if (item == null || item.GetType().IsValueType)
            {
                var compared = creator(item);
                return compared;
            }

            ICompared existing;
            if (this.map.TryGetValue(item, out existing))
            {
                return existing;
            }
            else
            {
                var compared = creator(item);
                this.map.Add(item, compared);
                return compared;
            }
        }
    }
}