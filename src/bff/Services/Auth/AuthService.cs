using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using API.DTOs;
using BFF.Services.Auth.DTO.Authentication;
using BFF.Services.Auth.DTO.Refresh;
using BFF.Services.Auth.DTO.Registration;

namespace BFF.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _client;
        private readonly ICurrentUserService _currentUserService;

        public AuthService(HttpClient client, ICurrentUserService currentUserService)
        {
            _client = client;
            _currentUserService = currentUserService;
        }

        public async Task<UserDto> Register(RegisterDto data)
        {
            var request = new RegistrationRequest()
            {
                UserName = data.Username,
                FirstName = "Demo",
                LastName = "Demo",
                Email = data.Email,
                Password = data.Password,
            };

            using StringContent content = new(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var resultMessage = await _client.PostAsync($"api/account/register", content);
            resultMessage.EnsureSuccessStatusCode();
            var result = await resultMessage.Content.ReadFromJsonAsync<RegistrationResponse>();
            if (result == null)
            {
                throw new InvalidOperationException("Error in create register");
            }

            return new UserDto
            {
                Id = result.UserId,
                Email = result.Email,
            };
        }

        public async Task<UserDto> Login(LoginDto data)
        {
            var request = new AuthenticationRequest()
            {
                Email = data.Username,
                Password = data.Password,
            };

            using StringContent content = new(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var resultMessage = await _client.PostAsync($"api/account/authenticate", content);
            resultMessage.EnsureSuccessStatusCode();
            var result = await resultMessage.Content.ReadFromJsonAsync<AuthenticationResponse>();
            if (result == null)
            {
                throw new InvalidOperationException("Error in login user");
            }

            return new UserDto
            {
                Id = result.Id,
                Email = result.Email,
                Token = result.Token,
            };
        }

        public async Task<UserDto> Refresh(RefreshDto data)
        {
            var request = new RefreshRequest()
            {
                UserId = _currentUserService.UserId,
                Code = data.Code,
            };

            using StringContent content = new(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", data.Token);

            var resultMessage = await _client.PostAsync($"api/account/refresh", content);
            resultMessage.EnsureSuccessStatusCode();
            var result = await resultMessage.Content.ReadFromJsonAsync<RefreshResponse>();
            if (result == null)
            {
                throw new InvalidOperationException("Error in refresh user");
            }

            return new UserDto
            {
                Id = result.Id,
                Email = result.Email,
                Token = result.Token,
            };
        }
    }
}
