using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mini.CoWorkify.Application.DTOs;
using Mini.CoWorkify.Application.Services;

namespace Mini.CoWorkify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReservationsController(IReservationsService service, IValidator<CreateReservationDto> validator)
    : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> Create(CreateReservationDto dto)
    {
        var validationResult = await validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToDictionary());
        
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            return Unauthorized(); 
        
        try 
        {
            var id = await service.CreateReservationAsync(dto, userId);
            return Ok(new { Id = id });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { Error = ex.Message });
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try 
        {
            var reservation = await service.GetReservationByIdAsync(id);
            
            if (reservation is null)
                return NotFound();
            
            return Ok(reservation);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}