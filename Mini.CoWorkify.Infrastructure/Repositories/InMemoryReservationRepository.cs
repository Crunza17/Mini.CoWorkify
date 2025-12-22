using Mini.CoWorkify.Domain.Entities;
using Mini.CoWorkify.Domain.Interfaces;

namespace Mini.CoWorkify.Infrastructure.Repositories;

public class InMemoryReservationRepository : IReservationRepository
{
    private static readonly List<Reservation> _database = [];
    public Task AddAsync(Reservation reservation)
    {
        _database.Add(reservation);

        return Task.CompletedTask;
    }

    public Task<Reservation?> GetByIdAsync(Guid reservationId)
    {
        var result = _database.FirstOrDefault(x => x.Id == reservationId);
        return Task.FromResult(result);
    }

    public Task<bool> IsDateOccupiedAsync(DateTime date)
    {
        var isOccupied = _database.Any(r => r.Date == date);
        return Task.FromResult(isOccupied);
    }
}