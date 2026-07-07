namespace SimpleWorkbench.Api.Infrastructure.Persistence.Records;

public sealed class InlineSecretRecord
{
    public string Id { get; set; } = string.Empty;
    public string NoteId { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string SecretValue { get; set; } = string.Empty;
}
