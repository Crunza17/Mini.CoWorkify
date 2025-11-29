namespace Mini.CoWorkify.Domain.Entities;


public class Reservation
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime Date { get; private set; }  
    public DateTime CreatedAt { get; private set; }
    
    public Reservation(Guid userId, DateTime date)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("The UserId is required", nameof(userId));
        
        if (date < DateTime.UtcNow)
            throw new ArgumentException("The reservation date is not valid", nameof(date));

        Id = Guid.NewGuid();
        UserId = userId;
        Date = date;
        CreatedAt = DateTime.UtcNow;
    }
}