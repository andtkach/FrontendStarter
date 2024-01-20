using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using API.DTOs;
using BFF.Data;
using BFF.DTOs;
using BFF.Services.Auth.DTO.Authentication;
using BFF.Services.Auth.DTO.Refresh;
using BFF.Services.Auth.DTO.Registration;

namespace BFF.Services.Auth
{
    public class AuthService : BaseService,  IAuthService
    {
        private readonly ILogger<AuthService> _logger;

        public AuthService(HttpClient client, ICurrentUserService currentUserService,
            IConfiguration configuration, ILogger<AuthService> logger)
        : base(client, currentUserService, configuration, logger)
        {
            _logger = logger;
        }

        public async Task<UserDto> Register(RegisterDto data)
        {
            _logger.LogInformation("Register call");
            var request = AuthServiceObjectsBuilder.BuildRegistrationRequest(data);
            
            using StringContent content = new(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                Constants.HttpMediaType);

            AddRequestHeaders();
            var resultMessage = await Client.PostAsync(Constants.HttpUrlRegister, content);
            await VerifyResponse(resultMessage);

            var result = await resultMessage.Content.ReadFromJsonAsync<RegistrationResponse>();
            VerifyResult(result, "Error in register user");

            return new UserDto
            {
                Id = result.UserId,
                Email = result.Email,
            };
        }

        public async Task<UserDto> Login(LoginDto data)
        {
            _logger.LogInformation("Login call");
            var request = AuthServiceObjectsBuilder.BuildAuthenticationRequest(data);
            
            using StringContent content = new(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                Constants.HttpMediaType);

            AddRequestHeaders();
            var resultMessage = await Client.PostAsync(Constants.HttpUrlAuthenticate, content);
            await VerifyResponse(resultMessage);
            var result = await resultMessage.Content.ReadFromJsonAsync<AuthenticationResponse>();
            VerifyResult(result, "Error in login user");

            return AuthServiceObjectsBuilder.BuildUserDto(result);
        }

        public async Task<UserDto> Refresh(RefreshDto data)
        {
            _logger.LogInformation("Refresh call");
            var request = AuthServiceObjectsBuilder.BuildRefreshRequest(data);
            
            using StringContent content = new(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                Constants.HttpMediaType);

            AddRequestHeaders();
            AddRequestAuth(data.Token);
            
            var resultMessage = await Client.PostAsync(Constants.HttpUrlRefresh, content);
            await VerifyResponse(resultMessage);
            var result = await resultMessage.Content.ReadFromJsonAsync<RefreshResponse>();
            VerifyResult(result, "Error in refresh user");
            return AuthServiceObjectsBuilder.BuildUserDto(result);
            
        }
    }
}
