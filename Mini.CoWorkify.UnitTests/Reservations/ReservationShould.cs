using Mini.CoWorkify.Domain.Entities;
using Shouldly;

namespace Mini.CoWorkify.UnitTests.Reservations;

public class ReservationShould
{
    [Fact]
    public void Be_Created_When_Data_Is_Valid()
    {
        var userId = Guid.NewGuid();
        var date = DateTime.UtcNow.AddDays(1);

        var reservation = new Reservation(userId, date);

        reservation.ShouldNotBeNull();
        reservation.Id.ShouldNotBe(Guid.Empty);
        reservation.UserId.ShouldBe(userId);
        reservation.Date.ShouldBe(date);
        reservation.CreatedAt.ShouldBe(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }
    
    [Fact]
    public void Throw_Exception_When_UserId_Is_Empty()
    {
        var userId = Guid.Empty;
        var date = DateTime.UtcNow.AddDays(1);

        var exception = Should.Throw<ArgumentException>(() => new Reservation(userId, date));

        exception.Message.ShouldContain("The UserId is required");
    }

    [Fact]
    public void Throw_Exception_When_Date_Is_Empty()
    {
        var userId = Guid.NewGuid();
        var date = DateTime.UtcNow.AddDays(-1);

        var exception = Should.Throw<ArgumentException>(() => new Reservation(userId, date));
        
        exception.Message.ShouldContain("The reservation date is not valid");
    }
}