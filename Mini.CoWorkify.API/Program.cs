using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mini.CoWorkify.Application.Services;
using Mini.CoWorkify.Domain.Interfaces;
using Mini.CoWorkify.Infrastructure.Data;
using Mini.CoWorkify.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining<Mini.CoWorkify.Application.Validators.CreateReservationDtoValidator>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<CoWorkifyDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IReservationRepository, SqlReservationRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();