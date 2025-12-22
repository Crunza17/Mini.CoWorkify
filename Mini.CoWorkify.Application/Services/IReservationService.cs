using Mini.CoWorkify.Application.DTOs;
using Mini.CoWorkify.Domain.Entities;

namespace Mini.CoWorkify.Application.Services;

public interface IReservationService
{
    Task<Guid> CreateReservationAsync(CreateReservationDto createReservationDto);
    Task<Reservation?> GetReservationByIdAsync(Guid id);
}