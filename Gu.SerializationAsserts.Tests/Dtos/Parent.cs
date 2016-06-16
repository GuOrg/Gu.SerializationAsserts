using System.Collections;
using System.Collections.Generic;

namespace Gu.SerializationAsserts.Tests.Dtos
{
    public class Parent
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

        public Parent(params Child[] children)
        {
            this.AddRange(children);
        }

        public int Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public IReadOnlyList<Child> Children => this.children;

        public void AddRange(Child[] newChildren)
        {
            foreach (var child in newChildren)
            {
                this.Add(child);
            }
        }

        public void Add(Child child)
        {
            this.children.Add(child);
            child.Parent = this;
        }
    }
}
