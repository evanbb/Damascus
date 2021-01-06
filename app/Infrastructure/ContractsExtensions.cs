using System;
using System.Collections.Generic;
using System.Linq;

namespace Damascus.Example.Api
{
    public static class ContractsExtensions
    {
        public static Domain.Widget ToDomain(this Contracts.Widget widget)
        {
            return Domain.Widget.Rehydrate(
                widget.Id,
                widget.Description,
                widget.Gears.ToDomain()
            );
        }

        public static Domain.Motor ToDomain(this IEnumerable<Contracts.Gear> gears)
        {
            return new Domain.Motor(gears.Select(g => g.ToDomain()));
        }

        public static Domain.Gear ToDomain(this Contracts.Gear gear)
        {
            return new Domain.Gear(gear.Teeth, new Domain.Measurement(
                        Enum.IsDefined(typeof(Domain.UnitType), gear.RadiusUnitType)
                            ? Enum.Parse<Domain.UnitType>(gear.RadiusUnitType)
                            : Domain.UnitType.Unspecified,
                        gear.Radius
                    ));
        }
    }
}
