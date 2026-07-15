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

            var savedNotes = await db.Notes
                .Where(x => x.IsSaved)
                .OrderBy(x => x.Title)
                .Select(x => new HomeItem(x.Id, x.Title))
                .ToListAsync();

            var recentNoteRows = await db.Notes
                .Where(x => x.LastViewedAt != null)
                .Select(x => new { x.Id, x.Title, x.LastViewedAt })
                .ToListAsync();
            var recentNotes = recentNoteRows
                .OrderByDescending(x => x.LastViewedAt)
                .Select(x => new HomeItem(x.Id, x.Title))
                .Take(20)
                .ToList();

            var globalNotes = await db.Notes
                .Where(x => x.SpaceId == null)
                .OrderBy(x => x.Title)
                .Select(x => new HomeItem(x.Id, x.Title))
                .ToListAsync();

            var smartFilters = new SmartFiltersResponse(
                HasSaved: savedNotes.Count > 0,
                Tags: [],
                Priorities: [],
                Statuses: []);

            return Results.Ok(new HomeResponse(spaces, savedNotes, recentNotes, globalNotes, smartFilters));
        });

        return app;
    }

    public sealed record HomeItem(string Id, string Title);
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
