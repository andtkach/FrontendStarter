namespace BFF.Services.Auth.DTO.Authentication
{
    public class AuthenticationResponse
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string CacheId { get; set; } = string.Empty;
    }
}
