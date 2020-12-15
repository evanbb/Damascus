using System;

namespace Damascus.Example.Domain
{
    public class Measurement
    {
        public Measurement(UnitType unitType, decimal amount)
        {
            if (unitType == UnitType.Unspecified)
            {
                throw new InvalidOperationException("Unrecognized unit type");
            }

            if (amount <= 0)
            {
                throw new InvalidOperationException("Amount must be greater than zero");
            }

            UnitType = unitType;
            Amount = amount;
        }

        public override string ToString()
        {
            return $"Measurement: {Amount} {UnitType}(s)";
        }

        public UnitType UnitType { get; }
        public decimal Amount { get; }
    }
}
