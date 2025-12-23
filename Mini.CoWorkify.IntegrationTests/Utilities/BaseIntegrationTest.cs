using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Mini.CoWorkify.Application.DTOs;
using Mini.CoWorkify.Infrastructure.Data;

namespace Mini.CoWorkify.IntegrationTests.Utilities;

public abstract class BaseIntegrationTest : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    protected readonly CustomWebApplicationFactory _factory;
    protected readonly HttpClient _client;

    protected BaseIntegrationTest(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CoWorkifyDbContext>();

        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
    
    protected async Task ExecuteDbContextAsync(Func<CoWorkifyDbContext, Task> action)
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CoWorkifyDbContext>();
        await action(context);
    }
    
    protected async Task AuthenticateAsync()
    {
        const string email = "testuser@coworkify.com";
        const string password = "Password123!";

        await _client.PostAsJsonAsync("/api/auth/register", new RegisterUserDto 
        { 
            Email = email, 
            Password = password 
        });

        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new LoginUserDto 
        { 
            Email = email, 
            Password = password 
        });

        loginResponse.EnsureSuccessStatusCode();

        var loginContent = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        var token = loginContent?.Token;

        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);
    }

    protected class LoginResponse
    {
        public string Token { get; init; } = string.Empty;
    }
}
