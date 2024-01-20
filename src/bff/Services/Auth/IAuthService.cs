using API.DTOs;
using BFF.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BFF.Services.Auth
{
    public interface IAuthService
    {
        Task<UserDto> Register(RegisterDto data);
        Task<UserDto> Login(LoginDto data);
        Task<UserDto> Refresh(RefreshDto data);
    }
}
