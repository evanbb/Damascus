using Microsoft;
using System;

namespace Damascus.Core
{
    public static class ObjectExtensions
    {
        public static void Switch<T>(this T obj, params (Func<T, bool>, Action<T>)[] cases)
        {
            foreach (var c in cases)
            {
                if (c.Item1(obj))
                {
                    c.Item2(obj);
                    return;
                }
            }
        }

        public static void When<T>(this T obj, params (Func<T, bool>, Action<T>)[] cases)
        {
            foreach (var c in cases)
            {
                if (c.Item1(obj))
                {
                    c.Item2(obj);
                }
            }
        }

        public static Maybe<T> ToMaybe<T>(this object obj)
        {
            if (obj is Maybe<T>)
            {
                return (Maybe<T>)obj;
            }

            if (obj is T)
            {
                return new Maybe<T>((T)obj);
            }

            return Maybe<T>.Nothing;
        }

        public static bool IsNull(this object obj)
        {
            return (obj is null);
        }

        public static bool IsNotNull(this object obj)
        {
            return !(obj is null);
        }
    }
}
