using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleWorkbench.Api.Infrastructure.Persistence;

namespace SimpleWorkbench.IntegrationTests.Api;

public sealed class TestApiFactory : WebApplicationFactory<SimpleWorkbench.Api.Program>
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"simple-workbench-tests-{Guid.NewGuid():N}.db");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, configBuilder) =>
        {
            configBuilder.AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    ["DatabaseProvider"] = "Sqlite",
                    ["ConnectionStrings:SimpleWorkbench"] = $"Data Source={_dbPath}"
                });
        });

        builder.ConfigureServices(services =>
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<SimpleWorkbenchDbContext>();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        });
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (File.Exists(_dbPath))
            {
                File.Delete(_dbPath);
            }
        }

        base.Dispose(disposing);
    }
}
