using System;

namespace Damascus.Core
{
    public struct Unit : IEquatable<Unit>
    {
        public static readonly Unit Value = new Unit();

        public override int GetHashCode()
        {
            return 0;
        }

        public bool Equals(Unit other)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is Unit;
        }

        public static bool operator ==(Unit a, Unit b)
        {
            return true;
        }

        public static bool operator !=(Unit a, Unit b)
        {
            return true;
        }
    }
}
