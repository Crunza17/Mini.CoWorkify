using System.Security.Claims;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mini.CoWorkify.API.Controllers;
using Mini.CoWorkify.Application.DTOs;
using Mini.CoWorkify.Application.Services;
using Moq;
using Shouldly;

namespace Mini.CoWorkify.UnitTests.Reservations;

public class ReservationsControllerShould
{
    private readonly Mock<IReservationsService> _serviceMock;
    private readonly Mock<IValidator<CreateReservationDto>> _validatorMock;
    private readonly ReservationsController _controller;

    public ReservationsControllerShould()
    {
        _serviceMock = new Mock<IReservationsService>();
        _validatorMock = new Mock<IValidator<CreateReservationDto>>();
        
        _controller = new ReservationsController(_serviceMock.Object, _validatorMock.Object);
    }

    private void SetupUserInContext(Guid userId)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, "test@test.com")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };
    }

    [Fact]
    public async Task ReturnOkWithId_When_TokenAndDataAreValid()
    {
        var date = DateTime.UtcNow.AddDays(1);
        var dto = new CreateReservationDto(date);
        var userId = Guid.NewGuid();
        var reservationId = Guid.NewGuid();

        _validatorMock.Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _serviceMock.Setup(s => s.CreateReservationAsync(dto, userId))
            .ReturnsAsync(reservationId);

        SetupUserInContext(userId);

        var result = await _controller.Create(dto);

        var okResult = result.ShouldBeOfType<OkObjectResult>();
        okResult.StatusCode.ShouldBe(200);

        _serviceMock.Verify(s => s.CreateReservationAsync(dto, userId), Times.Once);
    }

    [Fact]
    public async Task ReturnUnauthorized_When_TokenHasNoIdClaim()
    {
        var dto = new CreateReservationDto(DateTime.UtcNow.AddDays(1));

        _validatorMock.Setup(v => v.ValidateAsync(dto, CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext() 
        };

        var result = await _controller.Create(dto);

        result.ShouldBeOfType<UnauthorizedResult>(); 
        
        _serviceMock.Verify(s => s.CreateReservationAsync(It.IsAny<CreateReservationDto>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task ReturnBadRequest_When_ValidationFails()
    {
        var dto = new CreateReservationDto(DateTime.UtcNow); 

        var validationFailure = new ValidationResult([new ValidationFailure("Date", "Invalid date")]);
        _validatorMock.Setup(v => v.ValidateAsync(dto, CancellationToken.None))
            .ReturnsAsync(validationFailure);

        SetupUserInContext(Guid.NewGuid());

        var result = await _controller.Create(dto);

        result.ShouldBeOfType<BadRequestObjectResult>();
        
        _serviceMock.Verify(s => s.CreateReservationAsync(It.IsAny<CreateReservationDto>(), It.IsAny<Guid>()), Times.Never);
    }
}