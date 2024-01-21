namespace BFF.Services
{
    public interface ICurrentUserService
    {
        public string UserId { get; }
        public string UserName { get; }

        public string Token { get; }
    }
}
