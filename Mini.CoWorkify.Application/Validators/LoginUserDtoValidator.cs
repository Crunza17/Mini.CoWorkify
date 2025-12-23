using FluentValidation;
using Mini.CoWorkify.Application.DTOs;

namespace Mini.CoWorkify.Application.Validators;

public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
{
    public LoginUserDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email should not be empty")
            .EmailAddress().WithMessage("Email address is not valid");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password should not be empty");
    }
}