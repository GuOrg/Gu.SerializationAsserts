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
            var comparison = Comparison.CreateFor(expected, actual);
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
            var comparison = Comparison.CreateFor(p1, p2);
            var dump = Dump(comparison);
            Console.Write(dump);
            var expectedDump = "Level Expected: Level Actual: Level\r\n" +
                               "  <Value>k__BackingField: Int32 Expected: 1 Actual: 2\r\n" +
                               "  <Next>k__BackingField: Level Expected: Level Actual: Level\r\n" +
                               "    <Value>k__BackingField: Int32 Expected: 3 Actual: 4\r\n" +
                               "    <Next>k__BackingField: Level Expected: null Actual: null\r\n" +
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
        public void List()
        {
            var expected = new[] { 1, 2 };
            var actual = new[] { 3, 4 };
            var comparison = Comparison.CreateFor(expected, actual);
            var dump = Dump(comparison);
            Console.Write(dump);
            var expectedDump = "Int32[] Expected: Int32[] Actual: Int32[]\r\n" +
                               "  0: Int32 Expected: 1 Actual: 3\r\n" +
                               "  1: Int32 Expected: 2 Actual: 4";
            Assert.AreEqual(expectedDump, dump);
        }

        [Test]
        public void Parents()
        {
            var p1 = new Parent { new Child(1), new Child(2) };
            var p2 = new Parent { new Child(1), new Child(2) };
            var comparison = Comparison.CreateFor(p1, p2);
            var dump = Dump(comparison);
            Console.Write(dump);
        }

        private static string Dump(Comparison comparison)
        {
            using (var writer = new IndentedTextWriter(new StringWriter(), "  "))
            {
                Dump(comparison, writer);
                return writer.InnerWriter.ToString();
            }
        }

        private static void Dump(Comparison comparison, IndentedTextWriter writer)
        {
            if (comparison.Parent != null)
            {
                writer.WriteLine();
            }

            writer.Write($"{GetInfo(comparison)} " +
                         $"Expected: {GetValue(comparison.Expected)} " +
                         $"Actual: {GetValue(comparison.Actual)}");
            writer.Indent++;
            foreach (var child in comparison.GetChildren())
            {
                Dump(child, writer);
            }
            writer.Indent--;
        }

        private static string GetInfo(Comparison compared)
        {
            if (compared.Parent == null)
            {
                return compared.Type.Name;
            }

            var comparedField = compared.Expected as ComparedField;
            if (comparedField != null)
            {
                return $"{comparedField.ParentField.Name}: {comparedField.ParentField.FieldType.Name}";
            }

            return $"{((ComparedItem)compared.Expected).Index}: {compared.Type.Name}";
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
