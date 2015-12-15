namespace Gu.SerializationAsserts.Newtonsoft.Json.Tests.Dtos
{
    public class Dummy
    {
        public Dummy()
        {
        }

        public Dummy(int value)
        {
            this.Value = value;
        }

        public int Value { get; set; }
    }
}