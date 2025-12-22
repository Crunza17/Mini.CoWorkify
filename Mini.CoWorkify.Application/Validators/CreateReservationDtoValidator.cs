using FluentValidation;
using Mini.CoWorkify.Application.DTOs;

namespace Mini.CoWorkify.Application.Validators;

public class CreateReservationDtoValidator : AbstractValidator<CreateReservationDto>
{
    public CreateReservationDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId should not be empty");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date should not be empty")
            .GreaterThan(DateTime.Now).WithMessage("The reservation date should be after the current date");
    }
}