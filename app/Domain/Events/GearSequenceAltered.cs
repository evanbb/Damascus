using Damascus.Domain.Abstractions;

namespace Damascus.Example.Domain
{
    public class GearSequenceAltered : IDomainEvent
    {
        public GearSequenceAltered(Motor motor)
        {
            Motor = motor;
        }

        public Motor Motor { get; }
    }
}
