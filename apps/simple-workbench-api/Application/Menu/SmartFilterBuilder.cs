namespace SimpleWorkbench.Api.Application.Menu;

public static class SmartFilterBuilder
{
    public static SmartFiltersDto Build(IEnumerable<NoteSnapshot> notes)
    {
        var snapshots = notes.ToArray();

        return new SmartFiltersDto(
            Tags: snapshots
                .SelectMany(x => x.Tags)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
                .ToArray(),
            Priorities: snapshots
                .Select(x => x.Priority)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x!)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
                .ToArray(),
            Statuses: snapshots
                .Select(x => x.Status)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x!)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
                .ToArray(),
            HasSaved: snapshots.Any(x => x.IsSaved));
    }
}

public sealed record NoteSnapshot(
    IReadOnlyList<string> Tags,
    string? Priority,
    string? Status,
    bool IsSaved);

public sealed record SmartFiltersDto(
    IReadOnlyList<string> Tags,
    IReadOnlyList<string> Priorities,
    IReadOnlyList<string> Statuses,
    bool HasSaved);
