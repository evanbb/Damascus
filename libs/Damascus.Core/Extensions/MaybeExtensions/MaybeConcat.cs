using System;

namespace Damascus.Core
{
    public static partial class MaybeExtensions
    {
        public static Maybe<(T1, T2)> Concat<T1, T2>(this Maybe<T1> maybe, Func<T1, Maybe<T2>> selector)
        {
            selector.BetterNotBeNull("Selector");
            
            if (!maybe.HasValue)
            {
                return Maybe.Nothing;
            }
            
            var output = selector(maybe.Value);
            
            if (!output.HasValue)
            {
                return Maybe.Nothing;
            }
            
            var Item1 = maybe.Value;
            
            return (Item1, output.Value);
        }

        public static Maybe<(T1, T2, T3)> Concat<T1, T2, T3>(this Maybe<(T1, T2)> maybe, Func<(T1, T2), Maybe<T3>> selector)
        {
            selector.BetterNotBeNull("Selector");
            
            if (!maybe.HasValue)
            {
                return Maybe.Nothing;
            }
            
            var output = selector(maybe.Value);
            
            if (!output.HasValue)
            {
                return Maybe.Nothing;
            }
            
            var (Item1, Item2) = maybe.Value;
            
            return (Item1, Item2, output.Value);
        }

        public static Maybe<(T1, T2, T3, T4)> Concat<T1, T2, T3, T4>(this Maybe<(T1, T2, T3)> maybe, Func<(T1, T2, T3), Maybe<T4>> selector)
        {
            selector.BetterNotBeNull("Selector");
            
            if (!maybe.HasValue)
            {
                return Maybe.Nothing;
            }
            
            var output = selector(maybe.Value);
            
            if (!output.HasValue)
            {
                return Maybe.Nothing;
            }
            
            var (Item1, Item2, Item3) = maybe.Value;
            
            return (Item1, Item2, Item3, output.Value);
        }

        public static Maybe<(T1, T2, T3, T4, T5)> Concat<T1, T2, T3, T4, T5>(this Maybe<(T1, T2, T3, T4)> maybe, Func<(T1, T2, T3, T4), Maybe<T5>> selector)
        {
            selector.BetterNotBeNull("Selector");
            
            if (!maybe.HasValue)
            {
                return Maybe.Nothing;
            }
            
            var output = selector(maybe.Value);
            
            if (!output.HasValue)
            {
                return Maybe.Nothing;
            }
            
            var (Item1, Item2, Item3, Item4) = maybe.Value;
            
            return (Item1, Item2, Item3, Item4, output.Value);
        }

        public static Maybe<(T1, T2, T3, T4, T5, T6)> Concat<T1, T2, T3, T4, T5, T6>(this Maybe<(T1, T2, T3, T4, T5)> maybe, Func<(T1, T2, T3, T4, T5), Maybe<T6>> selector)
        {
            selector.BetterNotBeNull("Selector");
            
            if (!maybe.HasValue)
            {
                return Maybe.Nothing;
            }
            
            var output = selector(maybe.Value);
            
            if (!output.HasValue)
            {
                return Maybe.Nothing;
            }
            
            var (Item1, Item2, Item3, Item4, Item5) = maybe.Value;
            
            return (Item1, Item2, Item3, Item4, Item5, output.Value);
        }

        public static Maybe<(T1, T2, T3, T4, T5, T6, T7)> Concat<T1, T2, T3, T4, T5, T6, T7>(this Maybe<(T1, T2, T3, T4, T5, T6)> maybe, Func<(T1, T2, T3, T4, T5, T6), Maybe<T7>> selector)
        {
            selector.BetterNotBeNull("Selector");
            
            if (!maybe.HasValue)
            {
                return Maybe.Nothing;
            }
            
            var output = selector(maybe.Value);
            
            if (!output.HasValue)
            {
                return Maybe.Nothing;
            }
            
            var (Item1, Item2, Item3, Item4, Item5, Item6) = maybe.Value;
            
            return (Item1, Item2, Item3, Item4, Item5, Item6, output.Value);
        }

        public static Maybe<(T1, T2, T3, T4, T5, T6, T7, T8)> Concat<T1, T2, T3, T4, T5, T6, T7, T8>(this Maybe<(T1, T2, T3, T4, T5, T6, T7)> maybe, Func<(T1, T2, T3, T4, T5, T6, T7), Maybe<T8>> selector)
        {
            selector.BetterNotBeNull("Selector");
            
            if (!maybe.HasValue)
            {
                return Maybe.Nothing;
            }
            
            var output = selector(maybe.Value);
            
            if (!output.HasValue)
            {
                return Maybe.Nothing;
            }
            
            var (Item1, Item2, Item3, Item4, Item5, Item6, Item7) = maybe.Value;
            
            return (Item1, Item2, Item3, Item4, Item5, Item6, Item7, output.Value);
        }

        public static Maybe<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Concat<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Maybe<(T1, T2, T3, T4, T5, T6, T7, T8)> maybe, Func<(T1, T2, T3, T4, T5, T6, T7, T8), Maybe<T9>> selector)
        {
            selector.BetterNotBeNull("Selector");
            
            if (!maybe.HasValue)
            {
                return Maybe.Nothing;
            }
            
            var output = selector(maybe.Value);
            
            if (!output.HasValue)
            {
                return Maybe.Nothing;
            }
            
            var (Item1, Item2, Item3, Item4, Item5, Item6, Item7, Item8) = maybe.Value;
            
            return (Item1, Item2, Item3, Item4, Item5, Item6, Item7, Item8, output.Value);
        }

        public static Maybe<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Concat<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Maybe<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> maybe, Func<(T1, T2, T3, T4, T5, T6, T7, T8, T9), Maybe<T10>> selector)
        {
            selector.BetterNotBeNull("Selector");
            
            if (!maybe.HasValue)
            {
                return Maybe.Nothing;
            }
            
            var output = selector(maybe.Value);
            
            if (!output.HasValue)
            {
                return Maybe.Nothing;
            }
            
            var (Item1, Item2, Item3, Item4, Item5, Item6, Item7, Item8, Item9) = maybe.Value;
            
            return (Item1, Item2, Item3, Item4, Item5, Item6, Item7, Item8, Item9, output.Value);
        }
    }
}