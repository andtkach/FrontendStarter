using Jiwebapi.Catalog.Application.Models;
using Newtonsoft.Json;

namespace Jiwebapi.Catalog.Api.Utility.Extensions
{
    internal static class HttpContextExtensions
    {
        public static void AddResponseMeta(this HttpContext context, BaseVmResponse response)
        {
            if (!string.IsNullOrEmpty(response.Meta))
            {
                context.Response?.Headers?.Append("x-meta", response.Meta);
            }

            var str = JsonConvert.SerializeObject(response.Data);
            context.Response?.Headers?.Append("x-size", str.Length.ToString());
        }
    }
}
