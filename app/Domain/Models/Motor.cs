using System;
using System.Linq;
using System.Collections.Generic;
using Damascus.Core;

namespace Damascus.Example.Domain
{
    public class Motor
    {
        public Motor(IEnumerable<Gear> gears)
        {
            if (gears.IsEmpty())
            {
                throw new ArgumentNullException(nameof(gears), "Gears must be provided");
            }

            // every gear must be unique? maybe that's a thing?
            try
            {
                Gears = gears
                    .ToDictionary(gear => $"{gear.Teeth}-{gear.Size.ToString()}")
                    .Select(kvp => kvp.Value);
            }
            catch(ArgumentException dupes)
            {
                throw new InvalidOperationException("All gears must be unique", dupes);
            }
        }

        public IEnumerable<Gear> Gears { get; }
    }
}
