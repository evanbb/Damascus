using System;

namespace Damascus.Domain.Abstractions
{
    public abstract class Entity<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        
    }
}
