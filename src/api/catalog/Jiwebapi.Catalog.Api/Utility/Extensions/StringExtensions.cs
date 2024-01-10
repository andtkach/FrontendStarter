namespace Jiwebapi.Catalog.Api.Utility.Extensions
{
    public static class StringExtensions
    {
        public static DateTime ConvertToDateTime(this string dateTime)
        {
            return DateTime.Parse(dateTime);
        }
        public static bool ConvertToBool(this string value)
        {
            return bool.Parse(value);
        }
    }
}
