namespace Gu.SerializationAsserts.Tests.Dtos
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class Level
    {
        private int value;
        private Level next;

        public Level(int value)
        {
            this.Value = value;
        }

        public Level()
        {
        }

        public int Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public Level Next
        {
            get { return this.next; }
            set { this.next = value; }
        }

        public List<Level> Levels { get; set; } = new List<Level>();
    }
}
