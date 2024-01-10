namespace Jiwebapi.Catalog.Application.Models.Authentication
{
    public class RefreshRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
