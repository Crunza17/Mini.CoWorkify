using Mini.CoWorkify.Domain.Entities;

namespace Mini.CoWorkify.Domain.Interfaces;

public interface IReservationRepository
{
    Task AddAsync(Reservation reservation);
    Task<Reservation?> GetByIdAsync(Guid reservationId);
}