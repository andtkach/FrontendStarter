using System.Security.Claims;
using Jiwebapi.Catalog.Application.Contracts;

namespace Jiwebapi.Catalog.Api.Services
{
    public class LoggedInUserService : ILoggedInUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        
        public LoggedInUserService(IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor;
        }

        public string UserId
        {
            get
            {
                var id = _contextAccessor.HttpContext?.User?.FindFirstValue("uid");
                return id ?? Guid.Empty.ToString();
            }
        }

        public string UserName
        {
            get
            {
                var name = _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                return name ?? "Unknown";
            }
        }

        public string DataTraceId
        {
            get
            {
                var context = _contextAccessor.HttpContext;
                if (context != null && context.Items.TryGetValue("DataTraceId", out var item))
                {
                    var dataTraceId = item?.ToString();
                    return dataTraceId ?? string.Empty;
                }

                return string.Empty; 
                
            }
        }
    }
}
