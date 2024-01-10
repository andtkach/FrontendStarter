using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jiwebapi.Catalog.Cache.Cache
{
    public static class Serialization
    {
        public static byte[] ToByteArray(this object obj)
        {
            if (obj == null)
            {
                return null;
            }

            return JsonSerializer.SerializeToUtf8Bytes(obj,
                new JsonSerializerOptions
                    { WriteIndented = false, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
        }

        public static T FromByteArray<T>(this byte[] byteArray) where T : class
        {
            if (byteArray == null)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(new ReadOnlySpan<byte>(byteArray));
        }

    }
}