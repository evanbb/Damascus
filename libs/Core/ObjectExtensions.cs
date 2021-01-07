namespace Damascus.Core
{
    public static class ObjectExtensions
    {
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
            return obj is null;
        }
    }
}
