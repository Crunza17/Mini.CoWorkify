using Microsoft.AspNetCore.Mvc;
using Mini.CoWorkify.Application.DTOs;
using Mini.CoWorkify.Application.Services;

namespace Mini.CoWorkify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _service;
    
    public ReservationsController(IReservationService service)
    {
        _service = service;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateReservationDto dto)
    {
        try 
        {
            var id = await _service.CreateReservationAsync(dto);
            
            return Ok(new { Id = id });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try 
        {
            var reservation = await _service.GetReservationByIdAsync(id);
            
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