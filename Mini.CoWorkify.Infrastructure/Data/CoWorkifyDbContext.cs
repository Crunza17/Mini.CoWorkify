using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mini.CoWorkify.Domain.Entities;

namespace Mini.CoWorkify.Infrastructure.Data;

public class CoWorkifyDbContext(DbContextOptions<CoWorkifyDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Reservation> Reservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.Date).IsRequired();
            
            entity.HasIndex(e => e.Date);
        });
    }
}