using System;
using System.CodeDom.Compiler;
using System.IO;
using Gu.SerializationAsserts.Tests.Dtos;
using NUnit.Framework;

namespace Gu.SerializationAsserts.Tests.Comparers.DeepEquals
{
    public class ComparisonTest
    {
        [Test]
        public void Dummies()
        {
            var expected = new Dummy(1);
            var actual = new Dummy(2);
            var comparison = DeepEqualsNode.CreateFor(expected, actual);
            var dump = Dump(comparison);
            Console.Write(dump);
            var expectedDump = "Dummy Expected: Dummy Actual: Dummy\r\n" +
                               "  value: Int32 Expected: 1 Actual: 2";
            Assert.AreEqual(expectedDump, dump);
        }

        [Test]
        public void Levels()
        {
            var p1 = new Level(1) { Next = new Level(3) };
            var p2 = new Level(2) { Next = new Level(4) };
            var comparison = DeepEqualsNode.CreateFor(p1, p2);
            var dump = Dump(comparison);
            Console.Write(dump);
            var expectedDump = "Level Expected: Level Actual: Level\r\n" +
                               "  value: Int32 Expected: 1 Actual: 2\r\n" +
                               "  next: Level Expected: Level Actual: Level\r\n" +
                               "    value: Int32 Expected: 3 Actual: 4\r\n" +
                               "    next: Level Expected: null Actual: null\r\n" +
                               "    <Levels>k__BackingField: List`1 Expected: List`1 Actual: List`1\r\n" +
                               "      _items: Level[] Expected: Level[] Actual: Level[]\r\n" +
                               "      _size: Int32 Expected: 0 Actual: 0\r\n" +
                               "      _version: Int32 Expected: 0 Actual: 0\r\n" +
                               "      _syncRoot: Object Expected: null Actual: null\r\n" +
                               "  <Levels>k__BackingField: List`1 Expected: List`1 Actual: List`1\r\n" +
                               "    _items: Level[] Expected: Level[] Actual: Level[]\r\n" +
                               "    _size: Int32 Expected: 0 Actual: 0\r\n" +
                               "    _version: Int32 Expected: 0 Actual: 0\r\n" +
                               "    _syncRoot: Object Expected: null Actual: null";
            Assert.AreEqual(expectedDump, dump);
        }

        [Test]
        public void ListsWithSameLength()
        {
            var expected = new[] { 1, 2 };
            var actual = new[] { 3, 4 };
            var comparison = DeepEqualsNode.CreateFor(expected, actual);
            var dump = Dump(comparison);
            Console.Write(dump);
            var expectedDump = "Int32[] Expected: Int32[] Actual: Int32[]\r\n" +
                               "  0: Int32 Expected: 1 Actual: 3\r\n" +
                               "  1: Int32 Expected: 2 Actual: 4";
            Assert.AreEqual(expectedDump, dump);
        }

        [Test]
        public void ListWithDifferentLengths()
        {
            var expected = new[] { 1, 2 };
            var actual = new[] { 3, 4, 5 };
            var comparison = DeepEqualsNode.CreateFor(expected, actual);
            var dump = Dump(comparison);
            Console.Write(dump);
            var expectedDump = "Int32[] Expected: Int32[] Actual: Int32[]\r\n" +
                               "  2: Object[] Expected: Object[] Actual: Object[]";
            Assert.AreEqual(expectedDump, dump);
        }

        [Test, Explicit]
        public void Parents()
        {
            var p1 = new Parent(new Child(1), new Child(2));
            var p2 = new Parent(new Child(1), new Child(2));
            var comparison = DeepEqualsNode.CreateFor(p1, p2);
            var dump = Dump(comparison);
            Console.Write(dump);
        }

        private static string Dump(DeepEqualsNode deepEqualsNode)
        {
            using (var writer = new IndentedTextWriter(new StringWriter(), "  "))
            {
                Dump(deepEqualsNode, writer);
                return writer.InnerWriter.ToString();
            }
        }

        private static void Dump(DeepEqualsNode deepEqualsNode, IndentedTextWriter writer)
        {
            if (deepEqualsNode.Parent != null)
            {
                writer.WriteLine();
            }

            writer.Write($"{GetInfo(deepEqualsNode)} " +
                         $"Expected: {GetValue(deepEqualsNode.Expected)} " +
                         $"Actual: {GetValue(deepEqualsNode.Actual)}");
            writer.Indent++;
            foreach (var child in deepEqualsNode.Children)
            {
                Dump(child, writer);
            }

            writer.Indent--;
        }

        private static string GetInfo(DeepEqualsNode compared)
        {
            if (compared.Parent == null)
            {
                return compared.Type.Name;
            }

            var expected = compared.Expected;
            var comparedField = expected as ComparedField;
            if (comparedField != null)
            {
                return $"{comparedField.ParentField.Name}: {comparedField.ParentField.FieldType.Name}";
            }

            var comparedItem = expected as ComparedItem;
            if (comparedItem != null)
            {
                return $"{comparedItem.Index}: {compared.Type.Name}";
            }

            var comparedIEnumerable = expected as ComparedIEnumerable;
            if (comparedIEnumerable != null)
            {
                return $"{comparedIEnumerable.Count}: {compared.Type.Name}";
            }

            throw new NotImplementedException();
        }

        private static string GetValue(ICompared compared)
        {
            var value = compared.Value;
            if (value == null)
            {
                return "null";
            }

            var type = value.GetType();
            if (type.IsEquatable())
            {
                return value.ToString();
            }

            return type.Name;
        }
    }
}
