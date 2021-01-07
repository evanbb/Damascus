using Microsoft;
using System;
using System.Collections.Generic;

namespace Damascus.Core
{
    public static class Assertions
    {
        public static void BetterBe<T>(this T obj, Func<T, bool> predicate, string message = "Failed validation")
        {
            predicate.BetterNotBeNull("Predicate");

            if (!predicate(obj))
            {
                throw new InvalidOperationException(message);
            }
        }

        public static void BetterNotBe<T>(this T obj, T value, string message)
        {
            var comparer = EqualityComparer<T>.Default;

            if (comparer.Equals(obj, value))
            {
                throw new InvalidOperationException(message);
            }
        }

        public static void BetterNotBe<T>(this T obj, Func<T, bool> predicate, string message)
        {
            predicate.BetterNotBeNull("Predicate");

            if (predicate(obj))
            {
                throw new InvalidOperationException(message);
            }
        }

        public static void BetterNotBeNull([ValidatedNotNull] this object obj, string? paramName = "Object", string? message = "Object must not be null")
        {
            if (obj.IsNull())
            {
                throw new ArgumentNullException(paramName, message);
            }
        }

        public static void BetterHaveNoNulls<T>([ValidatedNotNull] this IEnumerable<T> elements, string? paramName = "Elements", string? message = "Must not contain null values")
        {
            foreach (var element in elements)
            {
                if (element.IsNull())
                {
                    throw new ArgumentException(message, paramName);
                }
            }
        }
    }
}
