using System;

namespace Damascus.Core
{
  public static class ObjectExtensions
  {
    public static void Switch<T>(this T obj, params (Func<T, bool>, Action<T>)[] cases)
    {
      foreach (var @case in cases)
      {
        if (@case.Item1(obj))
        {
          @case.Item2(obj);
          return;
        }
      }
    }

    public static void When<T>(this T obj, params (Func<T, bool>, Action<T>)[] cases)
    {
      foreach (var @case in cases)
      {
        if (@case.Item1(obj))
        {
          @case.Item2(obj);
        }
      }
    }
  }
}
