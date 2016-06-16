namespace Gu.SerializationAsserts.Tests.Dtos
{
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