using SimpleWorkbench.Api.Domain.Common;

namespace SimpleWorkbench.Api.Domain.Spaces;

public sealed class Space : AuditableEntity
{
    public string Name { get; private set; } = string.Empty;

    private Space() { }

    public static Space Create(string id, string name) =>
        new() { Id = id, Name = name };
}
