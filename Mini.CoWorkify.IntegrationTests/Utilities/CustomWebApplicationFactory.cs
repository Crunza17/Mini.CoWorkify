using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Mini.CoWorkify.API;
using Mini.CoWorkify.Infrastructure.Data;
using Testcontainers.MsSql;

namespace Mini.CoWorkify.IntegrationTests.Utilities;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .Build();

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<CoWorkifyDbContext>>();
            services.RemoveAll<CoWorkifyDbContext>();

            services.AddDbContext<CoWorkifyDbContext>(options =>
            {
                var connectionString = _dbContainer.GetConnectionString();

                var sqlBuilder = new SqlConnectionStringBuilder(connectionString)
                {
                    InitialCatalog = "CoWorkifyTestDb"
                };

                options.UseSqlServer(sqlBuilder.ConnectionString);
            });
        });
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}