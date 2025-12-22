using Mini.CoWorkify.Domain.Entities;
using Mini.CoWorkify.Domain.Interfaces;
using Mini.CoWorkify.Infrastructure.Data;

namespace Mini.CoWorkify.Infrastructure.Repositories;

public class SqlReservationRepository(CoWorkifyDbContext context) : IReservationRepository
{
    public async Task AddAsync(Reservation reservation)
    {
        await context.Reservations.AddAsync(reservation);
        
        await context.SaveChangesAsync();
    }

    public async Task<Reservation?> GetByIdAsync(Guid reservationId)
    {
        return await context.Reservations.FindAsync(reservationId);
    }
}