using System.Collections;
using System.Collections.Generic;

namespace Gu.SerializationAsserts.Tests.Dtos
{
    public class Parent : IEnumerable<Child>
    {
        private readonly List<Child> children = new List<Child>();

        public Parent()
        {
        }

        public Parent(Child child)
        {
            this.Add(child);
        }

        public int Value { get; set; }

        public IReadOnlyList<Child> Children => this.children;

        public void Add(Child child)
        {
            this.children.Add(child);
            child.Parent = this;
        }

        public IEnumerator<Child> GetEnumerator() => this.children.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }

    public class Child
    {
        public Child(int value)
        {
            this.Value = value;
        }

        public int Value { get; }

        public Parent Parent { get; internal set; }
    }
}
