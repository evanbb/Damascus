using System;

namespace Damascus.Core
{
    public static partial class MaybeExtensions
    {
        private static readonly Maybe<Unit> Nothing = Maybe.Nothing;

        public static Maybe<T> ToMaybe<T>(this T obj)
        {
            if (obj is Maybe<T>)
            {
                return obj;
            }

            if (obj is T)
            {
                return new Maybe<T>(obj);
            }

            return Nothing;
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

            return Nothing;
        }

        public static Maybe<T> Do<T>(this Maybe<T> maybe, Action<T> onValueCallback)
        {
            return maybe.Do(onValueCallback, () => { });
        }

        public static Maybe<T> Do<T>(this Maybe<T> maybe, Action<T> onValueCallback, Action onNothingCallback)
        {
            onValueCallback.BetterNotBeNull("Callback");
            onNothingCallback.BetterNotBeNull("Callback");

            if (maybe.HasValue)
            {
                onValueCallback(maybe.Value);
                return maybe;
            }

            onNothingCallback();
            return Maybe.Nothing;
        }

        #region IEnumerable stuff

        public static Maybe<K> Select<T, K>(this Maybe<T> maybe, Func<T, K> callback)
        {
            callback.BetterNotBeNull("Callback");

            if (maybe.HasValue)
            {
                return callback(maybe.Value);
            }

            return Maybe.Nothing;
        }

        #endregion
    }
}
