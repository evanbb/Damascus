using System.Collections.Generic;

namespace Damascus.Example.Infrastructure
{
    public static class DomainExtensions
    {
        public static Contracts.Widget ToContract(this Domain.Widget widget)
        {
            return new Contracts.Widget(widget.Id, widget.Description, widget.Motor.ToContract());
        }

        public static IEnumerable<Contracts.Gear> ToContract(this Domain.Motor motor)
        {
            foreach (var gear in motor.Gears)
            {
                yield return gear.ToContract();
            }
        }

        public static Contracts.Gear ToContract(this Domain.Gear gear)
        {
            return new Contracts.Gear(gear.Teeth, gear.Size.Amount, gear.Size.UnitType.ToString());
        }
    }
}
