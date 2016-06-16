namespace Gu.SerializationAsserts.Tests.Dtos
{
    public class Dummy
    {
        private int value;

        public Dummy(int value)
        {
            this.Value = value;
        }

        public Dummy()
        {
        }

        public int Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }

    public class TwoValueDummy
    {
        public TwoValueDummy()
        {
        }

        public TwoValueDummy(string name, int value)
        {
            this.Name = name;
            this.Value = value;
        }

        public string Name { get; set; }
        public int Value { get; set; }
    }
}
