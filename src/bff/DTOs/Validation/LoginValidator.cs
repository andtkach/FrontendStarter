using API.DTOs;
using FluentValidation;

namespace BFF.DTOs.Validation;

public class LoginValidator: AbstractValidator<LoginDto> 
{
    public LoginValidator()
    {
        RuleFor(x => x.Username).NotNull().NotEmpty().Length(6, 50).WithMessage("Username in invalid");
        RuleFor(x => x.Password).NotNull().NotEmpty().Length(6, 50).WithMessage("Password in invalid");
    }
}