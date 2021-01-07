using System;
using System.Linq;
using System.Collections.Generic;
using Damascus.Core;
using Damascus.Domain.Abstractions;

namespace Damascus.Example.Domain
{
    public class Motor
    {
        public Motor(IEnumerable<Gear> gears)
        {
            if (gears.IsEmpty())
            {
                throw new ArgumentNullException(nameof(gears), "Gears must be provided.");
            }

            Gears = ValidateGearUniqueness(gears);
        }

        public IEnumerable<Gear> Gears { get; private set; }

        public IEnumerable<IDomainEvent> AddGear(Gear gear, int position)
        {
            var newGears = Gears.ToList();

            if (position > newGears.Count)
            {
                throw new InvalidOperationException($"Cannot insert gear outside of widget motor. Widget motor currently only has {newGears.Count} gears.");
            }

            newGears.Insert(position, gear);

            Gears = ValidateGearUniqueness(newGears);

            yield return new GearAdded(gear);
        }

        public IEnumerable<IDomainEvent> SwapGears(int positionOne, int positionTwo)
        {
            var invalidPositionOne = positionOne < 0;
            var invalidPositionTwo = positionTwo < 0;

            if (invalidPositionOne || invalidPositionTwo)
            {
                throw new InvalidOperationException($"Both positions to swap must be greater than zero.");
            }

            var gears = Gears.ToArray();

            var outOfBoundsPositionOne = positionOne >= gears.Length;
            var outOfBoundsPositionTwo = positionTwo >= gears.Length;

            if (outOfBoundsPositionOne || outOfBoundsPositionTwo)
            {
                throw new InvalidOperationException($"Cannot swap gears that are out of bounds." +
                    $"{(outOfBoundsPositionOne ? $" Position 1 ({positionOne}) was outside of the bounds of the motor (count of {gears.Length})." : string.Empty)}" +
                    $"{(outOfBoundsPositionTwo ? $" Position 2 ({positionTwo}) was outside of the bounds of the motor (count of {gears.Length})." : string.Empty)}");
            }

            var gearOne = gears[positionOne];
            var gearTwo = gears[positionTwo];

            gears[positionOne] = gearTwo;
            gears[positionTwo] = gearOne;

            Gears = gears;

            yield return new GearReplaced(positionOne, gearTwo);
            yield return new GearReplaced(positionTwo, gearOne);
            yield return new GearSequenceAltered(this);
        }

        private static IEnumerable<Gear> ValidateGearUniqueness(IEnumerable<Gear> gearsToValidate)
        {
            // every gear must be unique? maybe that's a thing?
            try
            {
                return gearsToValidate
                    .ToDictionary(gear => $"{gear.Teeth}-{gear.Size}")
                    .Select(kvp => kvp.Value);
            }
            catch (ArgumentException dupes)
            {
                throw new InvalidOperationException("All gears must be unique.", dupes);
            }
        }
    }
}
