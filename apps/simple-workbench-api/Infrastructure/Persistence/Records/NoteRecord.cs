namespace SimpleWorkbench.Api.Infrastructure.Persistence.Records;

public sealed class NoteRecord
{
    public string Id { get; set; } = string.Empty;
    public string? SpaceId { get; set; }
    public string Title { get; set; } = string.Empty;
}
