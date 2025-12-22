using System.Net;
using System.Net.Http.Json;
using Mini.CoWorkify.Application.DTOs;
using Mini.CoWorkify.IntegrationTests.Utilities;
using Shouldly;

namespace Mini.CoWorkify.IntegrationTests;

public class ReservationsIntegrationTests(CustomWebApplicationFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Create_Should_ReturnConflict_When_DateIsDoubleBooked()
    {
        var date = DateTime.UtcNow.AddDays(10); 
        var command = new CreateReservationDto(Guid.NewGuid(), date);

        var response1 = await _client.PostAsJsonAsync("/api/reservations", command);
        response1.EnsureSuccessStatusCode();

        var response2 = await _client.PostAsJsonAsync("/api/reservations", command);

        response2.StatusCode.ShouldBe(HttpStatusCode.Conflict);
    }
}