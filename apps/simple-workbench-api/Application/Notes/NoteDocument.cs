using System.Text.Json.Serialization;

namespace SimpleWorkbench.Api.Application.Notes;

public sealed record NoteDocument(
    [property: JsonPropertyName("items")] IReadOnlyList<NoteDocumentItem> Items);

public sealed record NoteDocumentItem(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("type")] string Type);
