namespace SimpleWorkbench.Api.Application.Notes.Validation;

public sealed record ValidationResult(bool IsValid, IReadOnlyList<string> Errors);
