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
            return new Domain.Motor(gears.Select(g =>
                new Domain.Gear(
                    g.Teeth,
                    new Domain.Measurement(
                        Enum.IsDefined(typeof(Domain.UnitType), g.RadiusUnitType)
                            ? Enum.Parse<Domain.UnitType>(g.RadiusUnitType)
                            : Domain.UnitType.Unspecified,
                        g.Radius
                    )
                )
            ));
        }
    }
}
