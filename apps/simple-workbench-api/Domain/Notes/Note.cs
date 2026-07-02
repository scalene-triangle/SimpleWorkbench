namespace SimpleWorkbench.Api.Domain.Notes;

public sealed class Note
{
    public string Id { get; private set; } = string.Empty;
    public string? SpaceId { get; private set; }
    public string Title { get; private set; } = string.Empty;

    private Note() { }

    public static Note CreateGlobal(string id, string title) =>
        new() { Id = id, Title = title, SpaceId = null };
}
