using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using SimpleWorkbench.Api.Infrastructure.Persistence;
using SimpleWorkbench.Api.Infrastructure.Persistence.Records;
using SimpleWorkbench.IntegrationTests.Api;

namespace SimpleWorkbench.IntegrationTests.Search;

public class LexicalSearchTests(TestApiFactory factory) : IClassFixture<TestApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly TestApiFactory _factory = factory;

    [Fact]
    public async Task LexicalSearch_ShouldReturnTitleExactMatchFirst()
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<SimpleWorkbenchDbContext>();

        db.Notes.AddRange(
            new NoteRecord { Id = "n1", Title = "Redis setup", SearchText = "setup guide", Version = 1 },
            new NoteRecord { Id = "n2", Title = "Caching notes", SearchText = "redis mentioned", Version = 1 });
        await db.SaveChangesAsync();

        var result = await _client.GetFromJsonAsync<SearchResponse>("/api/search/lexical?q=Redis setup");

        Assert.NotNull(result);
        Assert.NotEmpty(result!.Items);
        Assert.Equal("n1", result.Items.First().NoteId);
    }

    private sealed record SearchResponse(IReadOnlyList<SearchItem> Items);
    private sealed record SearchItem(string NoteId, string Title, double Score);
}
