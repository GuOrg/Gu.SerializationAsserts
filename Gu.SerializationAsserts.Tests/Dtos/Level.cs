namespace Gu.SerializationAsserts.Tests.Dtos
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class Level
    {
        public Level(int value)
        {
            this.Value = value;
        }

        public Level()
        {
        }

        public int Value { get; set; }

        public Level Next { get; set; }

        public List<Level> Levels { get; set; } = new List<Level>();
    }
}
