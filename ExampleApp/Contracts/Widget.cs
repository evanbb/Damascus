using System;
using System.Collections.Generic;

namespace Damascus.Example.Contracts
{
    public record WidgetData(string Description, IEnumerable<Gear> Gears);
    public record Widget(Guid Id, string Description, IEnumerable<Gear> Gears)
        : WidgetData(Description, Gears);
}
