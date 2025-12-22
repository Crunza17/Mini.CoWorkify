using Mini.CoWorkify.Application.DTOs;
using Mini.CoWorkify.Application.Services;
using Mini.CoWorkify.Domain.Entities;
using Mini.CoWorkify.Domain.Interfaces;
using Moq;
using Shouldly;

namespace Mini.CoWorkify.UnitTests;

public class ReservationServiceShould
{
    private readonly Mock<IReservationRepository> _mockRepo;
    private readonly IReservationService _service;

    public ReservationServiceShould()
    {
        _mockRepo = new Mock<IReservationRepository>();
        _service = new ReservationService(_mockRepo.Object);
    }

    [Fact]
    public async Task ReturnId_When_DtoIsValid()
    {
        var dto = new CreateReservationDto(Guid.NewGuid(), DateTime.UtcNow.AddDays(1));

        var resultId = await _service.CreateReservationAsync(dto);
        
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
        var dto = new CreateReservationDto(Guid.NewGuid(), DateTime.UtcNow.AddDays(1));
        
        _mockRepo.Setup(r => r.IsDateOccupiedAsync(dto.Date)).ReturnsAsync(true);

        await Should.ThrowAsync<InvalidOperationException>(async () => 
            await _service.CreateReservationAsync(dto)
        );
    
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<Reservation>()), Times.Never);
    }
}