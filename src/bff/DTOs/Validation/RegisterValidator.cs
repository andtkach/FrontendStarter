using API.DTOs;
using FluentValidation;

namespace BFF.DTOs.Validation;

public class RegisterValidator: AbstractValidator<RegisterDto> 
{
    public RegisterValidator()
    {
        RuleFor(x => x.Username).NotNull().NotEmpty().Length(6, 50).WithMessage("Username in invalid");
        RuleFor(x => x.Password).NotNull().NotEmpty().Length(6, 50).WithMessage("Password in invalid");
        RuleFor(x => x.Email).NotNull().NotEmpty().Length(6, 50).WithMessage("Email in invalid");
    }
}