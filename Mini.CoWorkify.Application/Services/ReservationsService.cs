using Mini.CoWorkify.Application.DTOs;
using Mini.CoWorkify.Domain.Entities;
using Mini.CoWorkify.Domain.Interfaces;

namespace Mini.CoWorkify.Application.Services;

public class ReservationsService(IReservationRepository repository) : IReservationsService
{
    public async Task<Guid> CreateReservationAsync(CreateReservationDto dto, Guid userId)
    {
        var isOccupied = await repository.IsDateOccupiedAsync(dto.Date);
        
        if (isOccupied)
            throw new InvalidOperationException("The date is already occupied");
        
        var reservation = new Reservation(userId, dto.Date);
        await repository.AddAsync(reservation);

        return reservation.Id;
    }

    public async Task<Reservation?> GetReservationByIdAsync(Guid id)
    {
        return await repository.GetByIdAsync(id);
    }
}