namespace SimpleWorkbench.Api.Application.Search;

public static class HybridSearchService
{
    public static IReadOnlyList<HybridSearchHit> Merge(
        IReadOnlyList<SearchScore> lexical,
        IReadOnlyList<SearchScore> semantic)
    {
        var lexicalMap = lexical.ToDictionary(x => x.NoteId, x => x.Score);
        var semanticMap = semantic.ToDictionary(x => x.NoteId, x => x.Score);

        var noteIds = lexicalMap.Keys
            .Concat(semanticMap.Keys)
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        var merged = new List<HybridSearchHit>(noteIds.Length);
        foreach (var noteId in noteIds)
        {
            lexicalMap.TryGetValue(noteId, out var lexicalScore);
            semanticMap.TryGetValue(noteId, out var semanticScore);

            var finalScore = (0.55 * lexicalScore) + (0.40 * semanticScore) + (0.05 * 0d);
            merged.Add(new HybridSearchHit(noteId, finalScore));
        }

        return merged
            .OrderByDescending(x => x.Score)
            .ToList();
    }
}

public sealed record SearchScore(string NoteId, double Score);
public sealed record HybridSearchHit(string NoteId, double Score);
