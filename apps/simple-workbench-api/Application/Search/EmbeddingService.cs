namespace SimpleWorkbench.Api.Application.Search;

public sealed class EmbeddingService
{
    public Task<float[]> CreateEmbeddingAsync(string text, CancellationToken cancellationToken = default)
    {
        // Placeholder embedding implementation for local development baseline.
        var embedding = string.IsNullOrWhiteSpace(text)
            ? Array.Empty<float>()
            : [text.Length];

        return Task.FromResult(embedding);
    }
}
