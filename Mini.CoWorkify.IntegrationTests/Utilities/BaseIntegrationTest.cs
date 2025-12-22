using Microsoft.Extensions.DependencyInjection;
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
    
    // How to use (Remove this once used): 
    // await ExecuteDbContextAsync(async db => {
    //     var count = await db.Reservations.CountAsync();
    //     count.ShouldBe(1);
    // });
}
