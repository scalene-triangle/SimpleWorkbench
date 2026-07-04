using System.Text.Json;
using SimpleWorkbench.Api.Application.Notes;

namespace SimpleWorkbench.Api.Application.Notes.Validation;

public static class NoteDocumentValidator
{
    private static readonly HashSet<string> AllowedTypes =
    [
        "plainText",
        "richText",
        "codeBlock",
        "link",
        "secret"
    ];

    public static ValidationResult Validate(string json)
    {
        NoteDocument? document;

        try
        {
            document = JsonSerializer.Deserialize<NoteDocument>(json);
        }
        catch (JsonException)
        {
            return new ValidationResult(false, ["Invalid JSON payload."]);
        }

        document ??= new NoteDocument([]);

        var errors = new List<string>();
        foreach (var item in document.Items)
        {
            if (!AllowedTypes.Contains(item.Type))
            {
                errors.Add($"Unknown type: {item.Type}");
            }
        }

        return new ValidationResult(errors.Count == 0, errors);
    }
}
