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
        await AuthenticateAsync();
        var date = DateTime.UtcNow.AddDays(10); 
        
        var command = new CreateReservationDto(date);

        var response1 = await _client.PostAsJsonAsync("/api/reservations", command);
        response1.EnsureSuccessStatusCode();

        var response2 = await _client.PostAsJsonAsync("/api/reservations", command);

        response2.StatusCode.ShouldBe(HttpStatusCode.Conflict);
    }
    [Fact]
    public async Task Create_Should_ReturnUnauthorized_When_NoToken()
    {
        var date = DateTime.UtcNow.AddDays(12);
        var command = new CreateReservationDto(date);

        var response = await _client.PostAsJsonAsync("/api/reservations", command);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}