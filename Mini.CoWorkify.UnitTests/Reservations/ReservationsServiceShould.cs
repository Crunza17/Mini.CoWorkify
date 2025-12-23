using Mini.CoWorkify.Application.DTOs;
using Mini.CoWorkify.Application.Services;
using Mini.CoWorkify.Domain.Entities;
using Mini.CoWorkify.Domain.Interfaces;
using Moq;
using Shouldly;

namespace Mini.CoWorkify.UnitTests.Reservations;

public class ReservationsServiceShould
{
    private readonly Mock<IReservationRepository> _mockRepo;
    private readonly IReservationsService _service;

    public ReservationsServiceShould()
    {
        _mockRepo = new Mock<IReservationRepository>();
        _service = new ReservationsService(_mockRepo.Object);
    }

    [Fact]
    public async Task ReturnId_When_DtoIsValid()
    {
        var dto = new CreateReservationDto(DateTime.UtcNow.AddDays(1));
        var userId = Guid.NewGuid();
        var resultId = await _service.CreateReservationAsync(dto, userId);
        
        resultId.ShouldNotBe(Guid.Empty);
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<Reservation>()), Times.Once);
    }

    [Fact]
    public async Task ReturnReservation_When_IdExists()
    {
        var id  = Guid.NewGuid();
        var reservation = new Reservation(id, DateTime.UtcNow.AddDays(1));
        
        _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(reservation);

        var result = await _service.GetReservationByIdAsync(id);
        
        result.ShouldNotBeNull();
        result.Id.ShouldBe(reservation.Id);
    }
    
    [Fact]
    public async Task ThrowException_When_DateIsOccupied()
    {
        var dto = new CreateReservationDto(DateTime.UtcNow.AddDays(1));
        var userId = Guid.NewGuid();
        
        _mockRepo.Setup(r => r.IsDateOccupiedAsync(dto.Date)).ReturnsAsync(true);

        await Should.ThrowAsync<InvalidOperationException>(async () => 
            await _service.CreateReservationAsync(dto, userId)
        );
    
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<Reservation>()), Times.Never);
    }
}