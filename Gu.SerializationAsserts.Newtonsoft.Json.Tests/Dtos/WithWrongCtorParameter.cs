namespace Gu.SerializationAsserts.Newtonsoft.Json.Tests.Dtos
{
    public class WithWrongCtorParameter
    {
        public WithWrongCtorParameter(string nameThatDoesNotMatch)
        {
            this.Name = nameThatDoesNotMatch;
        }

        public string Name { get; set; }
    }
}