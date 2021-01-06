using Damascus.Domain.Abstractions;

namespace Damascus.Example.Domain
{
    public class GearReplaced : IDomainEvent
    {
        public GearReplaced(int position, Gear newGear)
        {
            Position = position;
            NewGear = newGear;
        }

        public int Position { get; }
        public Gear NewGear { get; }
    }
}
