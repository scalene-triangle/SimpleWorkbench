using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using SimpleWorkbench.Api.Infrastructure.Persistence;
using SimpleWorkbench.Api.Infrastructure.Persistence.Records;

namespace SimpleWorkbench.IntegrationTests.Api;

public class InlineSecretEndpointsTests(TestApiFactory factory) : IClassFixture<TestApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly TestApiFactory _factory = factory;

    [Fact]
    public async Task GetSecretDefault_ShouldReturnMaskedValue()
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<SimpleWorkbenchDbContext>();

        db.Notes.Add(new NoteRecord { Id = "n1", Title = "Seeded note", Version = 1 });
        db.InlineSecrets.Add(new InlineSecretRecord
        {
            Id = "s1",
            NoteId = "n1",
            SecretKey = "DB_PASSWORD",
            SecretValue = "p@ss"
        });
        await db.SaveChangesAsync();

        var response = await _client.GetAsync("/api/notes/n1/secrets/s1");
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<SecretResponse>();
        Assert.NotNull(payload);
        Assert.Equal("******", payload!.MaskedValue);
    }

    private sealed record SecretResponse(string Id, string SecretKey, string MaskedValue);
}
