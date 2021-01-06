using Damascus.Domain.Abstractions;

namespace Damascus.Example.Domain
{
    public class GearAdded : IDomainEvent
    {
        public GearAdded(Gear gearThatWasAdded)
        {
            GearThatWasAdded = gearThatWasAdded;
        }

        public Gear GearThatWasAdded { get; }
    }
}
