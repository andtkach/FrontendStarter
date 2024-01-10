namespace BFF.Services.Auth.DTO.Refresh
{
    public class RefreshRequest
    {
        public string Code { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
}
