namespace SimpleWorkbench.Api.Infrastructure.Persistence.Records;

public sealed class NoteRecord
{
    public string Id { get; set; } = string.Empty;
    public string? SpaceId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string DocumentJson { get; set; } = """{"items":[]}""";
    public string SearchText { get; set; } = string.Empty;
    public int Version { get; set; } = 1;
    public bool IsSaved { get; set; }
    public DateTimeOffset? LastViewedAt { get; set; }
}
