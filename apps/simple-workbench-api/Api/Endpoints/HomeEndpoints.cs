using Microsoft.EntityFrameworkCore;
using SimpleWorkbench.Api.Infrastructure.Persistence;

namespace SimpleWorkbench.Api.Api.Endpoints;

public static class HomeEndpoints
{
    public static IEndpointRouteBuilder MapHomeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api");

        group.MapGet("/home", async (SimpleWorkbenchDbContext db) =>
        {
            var spaces = await db.Spaces
                .OrderBy(x => x.Name)
                .Select(x => new HomeSpace(x.Id, x.Name))
                .ToListAsync();

            var savedNoteRows = await db.Notes
                .Where(x => x.IsSaved)
                .OrderBy(x => x.Title)
                .Select(x => new { x.Id, x.Title, x.SearchText })
                .ToListAsync();
            var savedNotes = savedNoteRows
                .Select(x => new HomeItem(x.Id, x.Title, BuildPreview(x.SearchText)))
                .ToList();

            var recentNoteRows = await db.Notes
                .Where(x => x.LastViewedAt != null)
                .Select(x => new { x.Id, x.Title, x.SearchText, x.LastViewedAt })
                .ToListAsync();
            var recentNotes = recentNoteRows
                .OrderByDescending(x => x.LastViewedAt)
                .Select(x => new HomeItem(x.Id, x.Title, BuildPreview(x.SearchText)))
                .Take(20)
                .ToList();

            var globalNoteRows = await db.Notes
                .Where(x => x.SpaceId == null)
                .OrderBy(x => x.Title)
                .Select(x => new { x.Id, x.Title, x.SearchText })
                .ToListAsync();
            var globalNotes = globalNoteRows
                .Select(x => new HomeItem(x.Id, x.Title, BuildPreview(x.SearchText)))
                .ToList();

            var smartFilters = new SmartFiltersResponse(
                HasSaved: savedNotes.Count > 0,
                Tags: [],
                Priorities: [],
                Statuses: []);

            return Results.Ok(new HomeResponse(spaces, savedNotes, recentNotes, globalNotes, smartFilters));
        });

        return app;
    }

    private static string BuildPreview(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return string.Empty;
        }

        var trimmed = searchText.Trim();
        return trimmed.Length <= 120 ? trimmed : $"{trimmed[..117]}...";
    }

    public sealed record HomeItem(string Id, string Title, string Preview);
    public sealed record HomeSpace(string Id, string Name);
    public sealed record HomeResponse(
        IReadOnlyList<HomeSpace> Spaces,
        IReadOnlyList<HomeItem> SavedNotes,
        IReadOnlyList<HomeItem> RecentNotes,
        IReadOnlyList<HomeItem> GlobalNotes,
        SmartFiltersResponse SmartFilters);

    public sealed record SmartFiltersResponse(
        bool HasSaved,
        IReadOnlyList<string> Tags,
        IReadOnlyList<string> Priorities,
        IReadOnlyList<string> Statuses);
}
