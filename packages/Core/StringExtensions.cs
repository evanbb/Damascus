namespace Damascus.Core
{
    public static class StringExtensions
    {
        public static bool HasContent(this string input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }
    }
}
