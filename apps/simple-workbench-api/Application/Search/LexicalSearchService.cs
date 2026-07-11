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
            .Select(x => new { x.Id, x.Title, x.SearchText })
            .ToListAsync(cancellationToken);

        return candidates
            .Select(x => new SearchHit(
                x.Id,
                x.Title,
                ComputeScore(x.Title, x.SearchText, normalizedQuery)))
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
}

public sealed record SearchHit(string NoteId, string Title, double Score);
