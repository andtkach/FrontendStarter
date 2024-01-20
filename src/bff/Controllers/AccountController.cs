using API.Controllers;
using API.DTOs;
using BFF.DTOs;
using BFF.Services;
using BFF.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using FluentValidation.Results;

namespace BFF.Controllers;

public class AccountController : BaseApiController
{
    private readonly IAuthService _authService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IValidator<LoginDto> _loginValidator;
    private readonly IValidator<RegisterDto> _registerValidator;

    public AccountController(IAuthService authService, ICurrentUserService currentUserService,
        IValidator<LoginDto> loginValidator, IValidator<RegisterDto> registerValidator)
    {
        _authService = authService;
        _currentUserService = currentUserService;
        _loginValidator = loginValidator;
        _registerValidator = registerValidator;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var vr = await _loginValidator.ValidateAsync(loginDto);
        if (!vr.IsValid)
        {
            return BadRequest(vr.Errors);
        }
        var result = await _authService.Login(loginDto);
        return (result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> RegisterUser(RegisterDto registerDto)
    {
        var vr = await _registerValidator.ValidateAsync(registerDto);

        if (!vr.IsValid)
        {
            return BadRequest(vr.Errors);
        }
        var result = await _authService.Register(registerDto);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("currentUser/{code}")]
    public async Task<ActionResult<UserDto>> GetCurrentUser(string code)
    {
        var result = await _authService.Refresh(new RefreshDto() { Code = code, Token = _currentUserService.Token });
        return result;
    }

}