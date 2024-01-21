using Jiwebapi.Catalog.Application.Models.Authentication;

namespace Jiwebapi.Catalog.Application.Contracts.Identity
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<RegistrationResponse> RegisterAsync(RegistrationRequest request);
        Task<RefreshResponse> RefreshAsync(RefreshRequest request);
    }
}
