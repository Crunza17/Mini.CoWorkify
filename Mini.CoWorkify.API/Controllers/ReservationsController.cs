using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Mini.CoWorkify.Application.DTOs;
using Mini.CoWorkify.Application.Services;

namespace Mini.CoWorkify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController(IReservationService service, IValidator<CreateReservationDto> validator)
    : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> Create(CreateReservationDto dto)
    {
        var validationResult = await validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToDictionary());
        
        try 
        {
            var id = await service.CreateReservationAsync(dto);
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