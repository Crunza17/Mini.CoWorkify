using Mini.CoWorkify.Application.DTOs;
using Mini.CoWorkify.Domain.Entities;

namespace Mini.CoWorkify.Application.Services;

public interface IReservationsService
{
    Task<Guid> CreateReservationAsync(CreateReservationDto createReservationDto, Guid userId);
    Task<Reservation?> GetReservationByIdAsync(Guid id);
}