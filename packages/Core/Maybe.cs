using System;
using System.Collections.Generic;

namespace Damascus.Core
{
    public struct Maybe<T>
    {
        public static readonly Maybe<T> Nothing;

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

        public override int GetHashCode() => _hasValue
            ? _value.GetHashCode()
            : 0;

        private bool _equals(Maybe<T> other) => !_hasValue
            ? !other.HasValue
            : other.HasValue && other.Value.Equals(_value);
    }

    public static class MaybeExtensions
    {
        // todo: put some magic here
    }
}
