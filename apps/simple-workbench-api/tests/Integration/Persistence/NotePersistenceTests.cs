using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SimpleWorkbench.Api.Infrastructure.Persistence;
using SimpleWorkbench.Api.Infrastructure.Persistence.Records;
using Xunit;

namespace SimpleWorkbench.IntegrationTests.Persistence;

public class NotePersistenceTests
{
    [Fact]
    public async Task CanPersistGlobalNote()
    {
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<SimpleWorkbenchDbContext>()
            .UseSqlite(connection)
            .Options;

        await using var db = new SimpleWorkbenchDbContext(options);
        await db.Database.EnsureCreatedAsync();

        db.Notes.Add(new NoteRecord { Id = "n1", Title = "Global", SpaceId = null });
        await db.SaveChangesAsync();

        var loaded = await db.Notes.SingleAsync(x => x.Id == "n1");
        Assert.Null(loaded.SpaceId);
    }
}
