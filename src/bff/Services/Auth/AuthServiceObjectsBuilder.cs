using API.DTOs;
using BFF.DTOs;
using BFF.Services.Auth.DTO.Authentication;
using BFF.Services.Auth.DTO.Refresh;
using BFF.Services.Auth.DTO.Registration;

namespace BFF.Services.Auth;

public static class AuthServiceObjectsBuilder
{
    public static AuthenticationRequest BuildAuthenticationRequest(LoginDto data)
    {
        return new AuthenticationRequest()
        {
            Email = data.Username,
            Password = data.Password,
        };
    }

    public static UserDto BuildUserDto(AuthenticationResponse data)
    {
        return new UserDto
        {
            Id = data.Id,
            Email = data.Email,
            Token = data.Token,
        };
    }

    public static UserDto BuildUserDto(RefreshResponse data)
    {
        return new UserDto
        {
            Id = data.Id,
            Email = data.Email,
            Token = data.Token,
        };
    }

    public static RegistrationRequest BuildRegistrationRequest(RegisterDto data)
    {
        return new RegistrationRequest()
        {
            UserName = data.Username,
            FirstName = "Demo",
            LastName = "Demo",
            Email = data.Email,
            Password = data.Password,
        };
    }
    
    public static RefreshRequest BuildRefreshRequest(RefreshDto data)
    {
        return new RefreshRequest()
        {
            Code = data.Code
        };
    }
}