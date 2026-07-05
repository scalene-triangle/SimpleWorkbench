using System.Net;
using System.Net.Http.Json;

namespace SimpleWorkbench.IntegrationTests.Api;

public class NoteConcurrencyTests(TestApiFactory factory) : IClassFixture<TestApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task UpdateWithStaleVersion_ShouldReturnConflict()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/notes", new CreateNoteRequest("N"));
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<NoteResponse>();
        Assert.NotNull(created);

        var freshResponse = await _client.PutAsJsonAsync(
            $"/api/notes/{created!.Id}",
            new UpdateNoteRequest("v2", 1));
        freshResponse.EnsureSuccessStatusCode();

        var staleResponse = await _client.PutAsJsonAsync(
            $"/api/notes/{created.Id}",
            new UpdateNoteRequest("stale", 1));

        Assert.Equal(HttpStatusCode.Conflict, staleResponse.StatusCode);
    }

    private sealed record CreateNoteRequest(string Title);
    private sealed record UpdateNoteRequest(string Title, int Version);
    private sealed record NoteResponse(string Id, string Title, int Version);
}
