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

    [Fact]
    public async Task Update_ShouldDeriveSearchTextFromDocumentAndExcludeSecretValue()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/notes", new CreateNoteRequest("Secure note"));
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<NoteResponse>();
        Assert.NotNull(created);

        const string documentJson =
            """{"items":[{"type":"plainText","text":"redis setup"},{"type":"secret","key":"DB_PASSWORD","value":"abc123"}]}""";

        var updateResponse = await _client.PutAsJsonAsync(
            $"/api/notes/{created!.Id}",
            new UpdateNoteRequest("Secure note", created.Version, documentJson));
        updateResponse.EnsureSuccessStatusCode();

        var updated = await updateResponse.Content.ReadFromJsonAsync<NoteResponse>();
        Assert.NotNull(updated);
        Assert.Contains("redis setup", updated!.SearchText);
        Assert.Contains("DB_PASSWORD", updated.SearchText);
        Assert.DoesNotContain("abc123", updated.SearchText);
    }

    private sealed record CreateNoteRequest(string Title);
    private sealed record UpdateNoteRequest(
        string Title,
        int Version,
        string? DocumentJson = null,
        string? SearchText = null,
        bool? IsSaved = null);
    private sealed record NoteResponse(string Id, string Title, int Version, string DocumentJson, string SearchText, bool IsSaved);
}
