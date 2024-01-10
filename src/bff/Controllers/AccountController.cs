using API.Controllers;
using API.DTOs;
using BFF.Services;
using BFF.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BFF.Controllers;

public class AccountController : BaseApiController
{
    private readonly IAuthService _authService;
    private readonly ICurrentUserService _currentUserService;

    public AccountController(IAuthService authService, ICurrentUserService currentUserService)
    {
        _authService = authService;
        _currentUserService = currentUserService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var result = await _authService.Login(loginDto);
        return result;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> RegisterUser(RegisterDto registerDto)
    {
        var result = await _authService.Register(registerDto);
        return result;
    }

    [Authorize]
    [HttpGet("currentUser/{code}")]
    public async Task<ActionResult<UserDto>> GetCurrentUser(string code)
    {
        var result = await _authService.Refresh(new RefreshDto() { Code = code, Token = _currentUserService.Token });
        return result;
    }

}