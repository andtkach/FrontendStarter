using System.Security.Claims;

namespace BFF.Services
{
    public class CurrentUserService: ICurrentUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
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

        public string Token
        {
            get
            {
                if (_contextAccessor.HttpContext != null)
                {
                    _contextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out var token);
                    if (!string.IsNullOrEmpty(token))
                    {
                        return token.ToString().Split(" ")[1];
                    }
                }

                return string.Empty;
            }
        }
    }
}
