using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SimpleWorkbench.Api.Infrastructure.Persistence;

namespace SimpleWorkbench.Api.Application.Search;

public sealed class LexicalSearchService(SimpleWorkbenchDbContext db)
{
    public async Task<IReadOnlyList<SearchHit>> SearchAsync(string query, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return [];
        }

        var normalizedQuery = query.Trim().ToLowerInvariant();

        var candidates = await db.Notes
            .Where(x =>
                EF.Functions.Like(x.Title.ToLower(), $"%{normalizedQuery}%") ||
                EF.Functions.Like(x.SearchText.ToLower(), $"%{normalizedQuery}%"))
            .Select(x => new { x.Id, x.Title, x.SearchText, x.DocumentJson })
            .ToListAsync(cancellationToken);

        return candidates
            .Select(x => new SearchHit(
                x.Id,
                x.Title,
                ComputeScore(x.Title, x.SearchText, normalizedQuery),
                FindMatchedItemId(x.DocumentJson, normalizedQuery)))
            .OrderByDescending(x => x.Score)
            .ThenBy(x => x.Title)
            .Take(50)
            .ToList();
    }

    private static double ComputeScore(string title, string searchText, string normalizedQuery)
    {
        var titleNormalized = title.ToLowerInvariant();
        var searchTextNormalized = searchText.ToLowerInvariant();

        var score = 0d;
        if (titleNormalized == normalizedQuery)
        {
            score += 2.0d;
        }
        else if (titleNormalized.Contains(normalizedQuery, StringComparison.Ordinal))
        {
            score += 1.0d;
        }

        if (searchTextNormalized.Contains(normalizedQuery, StringComparison.Ordinal))
        {
            score += 0.5d;
        }

        return score;
    }

    private static string? FindMatchedItemId(string documentJson, string normalizedQuery)
    {
        if (string.IsNullOrWhiteSpace(documentJson) || string.IsNullOrWhiteSpace(normalizedQuery))
        {
            return null;
        }

        try
        {
            using var doc = JsonDocument.Parse(documentJson);
            if (!doc.RootElement.TryGetProperty("items", out var items) || items.ValueKind != JsonValueKind.Array)
            {
                return null;
            }

            foreach (var item in items.EnumerateArray())
            {
                if (item.ValueKind != JsonValueKind.Object)
                {
                    continue;
                }

                if (!item.TryGetProperty("id", out var idElement) || idElement.ValueKind != JsonValueKind.String)
                {
                    continue;
                }

                var itemId = idElement.GetString();
                if (string.IsNullOrWhiteSpace(itemId))
                {
                    continue;
                }

                if (item.TryGetProperty("text", out var textElement) && textElement.ValueKind == JsonValueKind.String)
                {
                    var text = textElement.GetString();
                    if (!string.IsNullOrWhiteSpace(text) &&
                        text.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase))
                    {
                        return itemId;
                    }
                }

                if (item.TryGetProperty("key", out var keyElement) && keyElement.ValueKind == JsonValueKind.String)
                {
                    var key = keyElement.GetString();
                    if (!string.IsNullOrWhiteSpace(key) &&
                        key.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase))
                    {
                        return itemId;
                    }
                }
            }
        }
        catch (JsonException)
        {
            return null;
        }

        return null;
    }
}

public sealed record SearchHit(string NoteId, string Title, double Score, string? MatchedItemId);
