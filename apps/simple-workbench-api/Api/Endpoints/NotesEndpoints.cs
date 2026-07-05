using Microsoft.EntityFrameworkCore;
using SimpleWorkbench.Api.Infrastructure.Persistence;
using SimpleWorkbench.Api.Infrastructure.Persistence.Records;

namespace SimpleWorkbench.Api.Api.Endpoints;

public static class NotesEndpoints
{
    public static IEndpointRouteBuilder MapNotesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api");

        group.MapPost("/notes", async (CreateNoteRequest request, SimpleWorkbenchDbContext db) =>
        {
            var note = new NoteRecord
            {
                Id = Guid.NewGuid().ToString("N"),
                Title = request.Title,
                Version = 1
            };

            db.Notes.Add(note);
            await db.SaveChangesAsync();

            return Results.Created($"/api/notes/{note.Id}", new NoteResponse(note.Id, note.Title, note.Version));
        });

        group.MapPut("/notes/{id}", async (string id, UpdateNoteRequest request, SimpleWorkbenchDbContext db) =>
        {
            var note = await db.Notes.SingleOrDefaultAsync(x => x.Id == id);
            if (note is null)
            {
                return Results.NotFound();
            }

            if (note.Version != request.Version)
            {
                return Results.Conflict(new { message = "Version conflict" });
            }

            note.Title = request.Title;
            note.Version += 1;
            await db.SaveChangesAsync();

            return Results.Ok(new NoteResponse(note.Id, note.Title, note.Version));
        });

        return app;
    }

    public sealed record CreateNoteRequest(string Title);
    public sealed record UpdateNoteRequest(string Title, int Version);
    public sealed record NoteResponse(string Id, string Title, int Version);
}
