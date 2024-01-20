using System.Text.Json;

namespace BloomBFF.Extensions;

public static class StringExtensions
{
    public static bool IsJson(this string source)
    {
        if (source == null)
            return false;

        try
        {
            JsonDocument.Parse(source);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}