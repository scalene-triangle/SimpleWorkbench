using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using SimpleWorkbench.Api.Infrastructure.Persistence;
using SimpleWorkbench.Api.Infrastructure.Persistence.Records;

namespace SimpleWorkbench.IntegrationTests.Api;

public class HomeEndpointsTests(TestApiFactory factory) : IClassFixture<TestApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly TestApiFactory _factory = factory;

    [Fact]
    public async Task GetHome_ShouldReturnGlobalSections()
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<SimpleWorkbenchDbContext>();

        db.Spaces.Add(new SpaceRecord { Id = "s1", Name = "Main Space" });
        db.Notes.AddRange(
            new NoteRecord { Id = "n1", Title = "Saved note", IsSaved = true, SearchText = "saved preview text", Version = 1 },
            new NoteRecord { Id = "n2", Title = "Recent note", SearchText = "recent preview text", Version = 1, LastViewedAt = DateTimeOffset.UtcNow },
            new NoteRecord { Id = "n3", Title = "Global note", SpaceId = null, SearchText = "global preview text", Version = 1 });
        await db.SaveChangesAsync();

        var response = await _client.GetAsync("/api/home");
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<HomeResponse>();
        Assert.NotNull(payload);
        Assert.Contains(payload!.SavedNotes, x => x.Title == "Saved note");
        Assert.Contains(payload.RecentNotes, x => x.Title == "Recent note");
        Assert.Contains(payload.GlobalNotes, x => x.Title == "Global note");
        Assert.Contains(payload.SavedNotes, x => x.Preview == "saved preview text");
        Assert.Contains(payload.RecentNotes, x => x.Preview == "recent preview text");
        Assert.Contains(payload.GlobalNotes, x => x.Preview == "global preview text");
    }

    private sealed record HomeItem(string Id, string Title, string Preview);
    private sealed record HomeSpace(string Id, string Name);
    private sealed record HomeResponse(
        IReadOnlyList<HomeSpace> Spaces,
        IReadOnlyList<HomeItem> SavedNotes,
        IReadOnlyList<HomeItem> RecentNotes,
        IReadOnlyList<HomeItem> GlobalNotes);
}
