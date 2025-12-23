using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Mini.CoWorkify.Application.DTOs;
using Mini.CoWorkify.Application.Services;
using Mini.CoWorkify.Application.Validators;

namespace Mini.CoWorkify.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AuthController(IAuthService service, IValidator<RegisterUserDto> registerValidator, IValidator<LoginUserDto> loginValidator)
    : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto dto)
    {
        var validationResult = await registerValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToDictionary());
        
        var errors = await service.RegisterAsync(dto);

        if (errors.Any())
        {
            return BadRequest(errors);
        }

        return Ok(new { Message = "User successfully registered" });
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto dto)
    {
        var validationResult = await loginValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToDictionary());
        
        var token = await service.LoginAsync(dto);

        if (token is null)
        {
            return Unauthorized(new { Message = "Invalid Credentials" });
        }

        return Ok(new { Token = token });
    }
}