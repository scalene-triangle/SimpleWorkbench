namespace SimpleWorkbench.Api.Domain.Common;

public abstract class AuditableEntity
{
    public string Id { get; protected set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; protected set; }
    public DateTimeOffset UpdatedAt { get; protected set; }
}
