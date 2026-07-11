using Microsoft.EntityFrameworkCore;
using SimpleWorkbench.Api.Application.Search;
using SimpleWorkbench.Api.Infrastructure.Persistence;

namespace SimpleWorkbench.Api.Application.Notes;

public sealed class SaveNoteCommandHandler(SimpleWorkbenchDbContext db)
{
    public async Task<bool> UpdateDocumentAsync(string noteId, string documentJson, CancellationToken cancellationToken = default)
    {
        var note = await db.Notes.SingleOrDefaultAsync(x => x.Id == noteId, cancellationToken);
        if (note is null)
        {
            return false;
        }

        note.DocumentJson = documentJson;
        note.SearchText = NoteSearchTextExtractor.Extract(documentJson);
        note.Version += 1;

        await db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
