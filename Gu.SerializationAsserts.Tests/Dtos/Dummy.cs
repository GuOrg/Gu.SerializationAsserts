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
}
