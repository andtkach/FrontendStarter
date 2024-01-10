namespace Jiwebapi.Catalog.Api.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
        
        public static IApplicationBuilder UseHistory(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HistoryMiddleware>();
        }
    }
}
