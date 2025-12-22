using Mini.CoWorkify.Application.DTOs;
using Mini.CoWorkify.Domain.Entities;
using Mini.CoWorkify.Domain.Interfaces;

namespace Mini.CoWorkify.Application.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _repository;

    public ReservationService(IReservationRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> CreateReservationAsync(CreateReservationDto dto)
    {
        var isOccupied = await _repository.IsDateOccupiedAsync(dto.Date);
        
        if (isOccupied)
            throw new InvalidOperationException("The date is already occupied");
        
        var reservation = new Reservation(dto.UserId, dto.Date);
        await _repository.AddAsync(reservation);

        return reservation.Id;
    }

    public async Task<Reservation?> GetReservationByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }
}