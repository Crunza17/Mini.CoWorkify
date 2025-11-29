using Mini.CoWorkify.Application.DTOs;
using Mini.CoWorkify.Domain.Interfaces;

namespace Mini.CoWorkify.Application.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _repository;

    public ReservationService(IReservationRepository repository)
    {
        _repository = repository;
    }

    public Task<Guid> CreateReservationAsync(CreateReservationDto createReservationDto)
    {
        throw new NotImplementedException();
    }
}