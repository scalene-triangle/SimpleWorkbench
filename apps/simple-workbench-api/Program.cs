using Microsoft.EntityFrameworkCore;
using SimpleWorkbench.Api.Api.Endpoints;
using SimpleWorkbench.Api.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SimpleWorkbench")
    ?? "Server=(localdb)\\mssqllocaldb;Database=SimpleWorkbench;Trusted_Connection=True;TrustServerCertificate=True";

var databaseProvider = builder.Configuration["DatabaseProvider"];
if (string.IsNullOrWhiteSpace(databaseProvider))
{
    databaseProvider = connectionString.TrimStart().StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase)
        ? "Sqlite"
        : "SqlServer";
}

builder.Services.AddDbContext<SimpleWorkbenchDbContext>(options =>
{
    if (string.Equals(databaseProvider, "Sqlite", StringComparison.OrdinalIgnoreCase))
    {
        options.UseSqlite(connectionString);
        return;
    }

    options.UseSqlServer(connectionString);
});

var app = builder.Build();
app.MapGet("/health", () => Results.Ok("ok"));
app.MapNotesEndpoints();
app.MapHomeEndpoints();
app.Run();

namespace SimpleWorkbench.Api
{
    public partial class Program { }
}
