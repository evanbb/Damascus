using System;

namespace Damascus.Core
{
    public struct Maybe<T> : IEquatable<Maybe<T>>, IEquatable<T>
    {
        private readonly T _value;
        private readonly bool _hasValue;

        public Maybe(T value)
        {
            _value = value;
            _hasValue = !(value is null);
        }

        public T Value => _hasValue
            ? _value
            : throw new InvalidOperationException($"Unable to get value for ${GetType().FullName}");

        public bool HasValue => _hasValue;

        public override bool Equals(object obj)
        {
            if (obj is Maybe<T>)
            {
                return _equals((Maybe<T>)obj);
            }

            if (obj is T)
            {
                return _equals(new Maybe<T>((T)obj));
            }

            return false;
        }

        public bool Equals(Maybe<T> maybe)
        {
            return _equals(maybe);
        }

        public bool Equals(T value)
        {
            return _equals(new Maybe<T>(value));
        }

        private bool _equals(Maybe<T> other) => !_hasValue
            ? !other.HasValue
            : other.HasValue && other.Value.Equals(_value);

        public override int GetHashCode() => _hasValue
            ? _value.GetHashCode()
            : 0;

        public static bool operator ==(Maybe<T> first, Maybe<T> second) => first.Equals(second);
        public static bool operator !=(Maybe<T> first, Maybe<T> second) => !first.Equals(second);

        public static bool operator ==(Maybe<T> first, T second) => first.Equals(second);
        public static bool operator !=(Maybe<T> first, T second) => !first.Equals(second);

        public static bool operator ==(T first, Maybe<T> second) => second.Equals(first);
        public static bool operator !=(T first, Maybe<T> second) => !second.Equals(first);

        public static implicit operator T(Maybe<T> maybe) => maybe.Value;
        public static implicit operator Maybe<T>(T value) => new Maybe<T>(value);
        public static implicit operator Maybe<T>(Maybe<Unit> _) => Maybe.Nothing;
    }

    public static class Maybe
    {
        public static readonly Maybe<Unit> Nothing = new Maybe<Unit>();
    }

    public static class MaybeExtensions
    {
        // todo: put some magic here
    }
}
