using System.Collections;
using System.Collections.Generic;

namespace Gu.SerializationAsserts.Tests.Dtos
{
    public class Parent : IEnumerable<Child>
    {
        private readonly List<Child> children = new List<Child>();

        private int value;

        public Parent()
        {
        }

        public Parent(Child child)
        {
            this.Add(child);
        }

        public int Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

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
        private readonly int value;
        private Parent parent;

        public Child(int value)
        {
            this.value = value;
        }

        public int Value
        {
            get { return this.value; }
        }

        public Parent Parent
        {
            get { return this.parent; }
            internal set { this.parent = value; }
        }
    }
}
