using Microsoft.EntityFrameworkCore;
using SimpleWorkbench.Api.Application.Search;
using SimpleWorkbench.Api.Api.Endpoints;
using SimpleWorkbench.Api.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SimpleWorkbench")
    ?? throw new InvalidOperationException("Connection string 'SimpleWorkbench' was not found.");

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
builder.Services.AddScoped<LexicalSearchService>();

var app = builder.Build();
app.MapGet("/health", () => Results.Ok("ok"));
app.MapNotesEndpoints();
app.MapHomeEndpoints();
app.MapSearchEndpoints();
app.Run();

namespace SimpleWorkbench.Api
{
    public partial class Program { }
}
