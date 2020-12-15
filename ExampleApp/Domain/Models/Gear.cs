using System;

namespace Damascus.Example.Domain
{
    public class Gear
    {
        public Gear(long teeth, Measurement size)
        {
            if (teeth <= 0)
            {
                throw new InvalidOperationException("Cannot create toothless gear");
            }

            Teeth = teeth;
            Size = size;
        }

        public long Teeth { get; }
        public Measurement Size { get; }
    }
}
