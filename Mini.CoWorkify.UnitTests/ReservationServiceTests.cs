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
    public async Task Call_Repository_And_Return_Id_When_Dto_Is_Valid()
    {
        var dto = new CreateReservationDto(Guid.NewGuid(), DateTime.UtcNow.AddDays(1));

        var resultId = await _service.CreateReservationAsync(dto);
        
        resultId.ShouldNotBe(Guid.Empty);
        
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<Reservation>()), Times.Once);
    }
}