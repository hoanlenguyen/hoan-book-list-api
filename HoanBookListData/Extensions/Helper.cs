namespace HoanBookListData.Extensions
{
    public static class Helper
    {
        public static bool HasValue(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }
    }
}