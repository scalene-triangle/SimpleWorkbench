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

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SimpleWorkbenchDbContext>();
    await db.Database.EnsureCreatedAsync();

    if (string.Equals(databaseProvider, "SqlServer", StringComparison.OrdinalIgnoreCase))
    {
        await db.Database.ExecuteSqlRawAsync("""
            IF COL_LENGTH('Note', 'IsSaved') IS NULL
            BEGIN
                ALTER TABLE [Note] ADD [IsSaved] bit NOT NULL CONSTRAINT [DF_Note_IsSaved] DEFAULT(0);
            END;

            IF COL_LENGTH('Note', 'LastViewedAt') IS NULL
            BEGIN
                ALTER TABLE [Note] ADD [LastViewedAt] datetimeoffset NULL;
            END;

            IF COL_LENGTH('Note', 'DocumentJson') IS NULL
            BEGIN
                ALTER TABLE [Note] ADD [DocumentJson] nvarchar(max) NOT NULL CONSTRAINT [DF_Note_DocumentJson] DEFAULT('{{"items":[]}}');
            END;

            IF COL_LENGTH('Note', 'SearchText') IS NULL
            BEGIN
                ALTER TABLE [Note] ADD [SearchText] nvarchar(max) NOT NULL CONSTRAINT [DF_Note_SearchText] DEFAULT('');
            END;
            """);
    }
}

app.MapGet("/health", () => Results.Ok("ok"));
app.MapNotesEndpoints();
app.MapHomeEndpoints();
app.MapSearchEndpoints();
app.Run();

namespace SimpleWorkbench.Api
{
    public partial class Program { }
}
