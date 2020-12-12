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
    }
}
