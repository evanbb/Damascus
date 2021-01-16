namespace Damascus.Core
{
    public static class ObjectExtensions
    {
        public static bool IsNull(this object obj)
        {
            return obj is null;
        }
    }
}
