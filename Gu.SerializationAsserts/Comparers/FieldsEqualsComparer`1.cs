namespace Gu.SerializationAsserts
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// A deep equals checking nested fields 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FieldsEqualsComparer<T> : IEqualityComparer<T>, IComparer
    {
        public static readonly FieldsEqualsComparer<T> Default = new FieldsEqualsComparer<T>();

        private static readonly FieldInfo[] Fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

        private FieldsEqualsComparer()
        {
        }

        public bool Equals(T x, T y)
        {
            return Equals(x, y, new List<object>(), new List<object>());
        }

        /// <summary>
        /// nUnit uses IComparer for CollectionAssert
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        int IComparer.Compare(object x, object y)
        {
            if (Equals((T)x, (T)y, new List<object>(), new List<object>()))
            {
                return 0;
            }
            return 1;
        }

        public bool Equals(T x, T y, List<object> checkedX, List<object> checkedY)
        {
            foreach (var fieldInfo in Fields)
            {
                var xValue = fieldInfo.GetValue(x);
                var yValue = fieldInfo.GetValue(y);
                if (xValue == null && yValue == null)
                {
                    continue;
                }

                if (checkedX.Any(item => ReferenceEquals(item, xValue)) && checkedY.Any(item => ReferenceEquals(item, yValue)))
                {
                    continue;
                }

                if (xValue == null || yValue == null)
                {
                    return false;
                }

                var xType = xValue.GetType();
                var yType = yValue.GetType();
                if (xType != yType)
                {
                    return false;
                }

                if (!xType.IsEquatable())
                {
                    checkedX.AddIfNotExists(xValue);
                    checkedY.AddIfNotExists(yValue);
                    if (IsAnyIEnumerable(xValue, yValue))
                    {
                        if (!EnumerableEquals(xValue as IEnumerable, yValue as IEnumerable, checkedX, checkedY))
                        {
                            return false;
                        }
                        continue;
                    }
                    if (!ReflectionEquals(xValue, yValue, checkedX, checkedY))
                    {
                        return false;
                    }

                    continue;
                }

                if (!Object.Equals(xValue, yValue))
                {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode(T obj)
        {
            throw new System.NotImplementedException();
        }

        internal static bool ReflectionEquals(object x, object y, List<object> checkedX, List<object> checkedY)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }
            var type = x.GetType();
            if (y.GetType() != type)
            {
                return false;
            }

            var comparerType = typeof(FieldsEqualsComparer<>).MakeGenericType(type);
            var defaultField = comparerType.GetField(nameof(Default), BindingFlags.Public | BindingFlags.Static);
            var comparer = defaultField.GetValue(null);
            var equalsMethod = comparerType.GetMethod(nameof(Equals), BindingFlags.Public | BindingFlags.Instance, null, new Type[] { type, type, typeof(List<object>), typeof(List<object>) }, null);
            return (bool)equalsMethod.Invoke(comparer, new[] { x, y, checkedX, checkedY });
        }

        internal static bool EnumerableEquals(IEnumerable x, IEnumerable y, List<object> checkedX, List<object> checkedY)
        {
            var xs = x.OfType<object>().ToArray();
            var ys = y.OfType<object>().ToArray();
            if (xs.Length != ys.Length)
            {
                return false;
            }
            for (int i = 0; i < Math.Max(xs.Length, ys.Length); i++)
            {
                if (!ReflectionEquals(xs.ElementAtOrDefault(i), ys.ElementAtOrDefault(i), checkedX, checkedY))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsAnyIEnumerable(object a, object b)
        {
            return a is IEnumerable || b is IEnumerable;
        }
    }
}
