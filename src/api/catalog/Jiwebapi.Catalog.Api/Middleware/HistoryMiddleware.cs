using System.Security.Claims;
using System.Text;
using Jiwebapi.Catalog.Api.Controllers;
using Jiwebapi.Catalog.Application.Contracts.Cache;
using Jiwebapi.Catalog.History.Entity;
using Newtonsoft.Json;

namespace Jiwebapi.Catalog.Api.Middleware
{
    public class HistoryMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHistoryPublisher _historyPublisher;
        private readonly ILogger<HistoryMiddleware> _logger;

        public HistoryMiddleware(RequestDelegate next, IHistoryPublisher historyPublisher, ILogger<HistoryMiddleware> logger)
        {
            _next = next;
            _historyPublisher = historyPublisher;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var dataTraceId = Guid.NewGuid().ToString();
            _logger.LogInformation($"DataTraceId: {dataTraceId}");

            try
            {
                context.Items.Add("DataTraceId", dataTraceId);
                context.Request.EnableBuffering();
                await _next(context);
            }
            finally
            {
                if (context.Request.Method == HttpMethods.Post
                    || context.Request.Method == HttpMethods.Put
                    || context.Request.Method == HttpMethods.Patch
                    || context.Request.Method == HttpMethods.Delete)
                {
                    string body = string.Empty;
                    if (context.Request.ContentLength > 0)
                    {
                        var request = context.Request;

                        request.Body.Position = 0;
                        
                        using (var reader = new StreamReader(
                                   context.Request.Body,
                                   encoding: Encoding.UTF8,
                                   detectEncodingFromByteOrderMarks: false,
                                   leaveOpen: true))
                        {
                            body = await reader.ReadToEndAsync();
                        }
                    }

                    var historyEntry = new PublicEntry
                    {
                        User = context.User?.FindFirstValue("uid"),
                        Action = context.Request.Method,
                        Path = context.Request.Path,
                        Data = body,
                        DataTraceId = dataTraceId
                };

                    await _historyPublisher.Publish(JsonConvert.SerializeObject(historyEntry));
                }
            }
        }
    }
}
