namespace Gu.SerializationAsserts.Tests.Dtos
{
    using System;

    [Serializable]
    public class Level
    {
        public int Value { get; set; }

        public Level Next { get; set; }
    }
}
