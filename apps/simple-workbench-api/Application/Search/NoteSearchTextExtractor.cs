using System.Text.Json;

namespace SimpleWorkbench.Api.Application.Search;

public static class NoteSearchTextExtractor
{
    public static string Extract(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return string.Empty;
        }

        JsonDocument document;
        try
        {
            document = JsonDocument.Parse(json);
        }
        catch (JsonException)
        {
            return string.Empty;
        }

        using (document)
        {
            if (!document.RootElement.TryGetProperty("items", out var items) ||
                items.ValueKind != JsonValueKind.Array)
            {
                return string.Empty;
            }

            var tokens = new List<string>();
            foreach (var item in items.EnumerateArray())
            {
                if (item.ValueKind != JsonValueKind.Object ||
                    !item.TryGetProperty("type", out var typeElement))
                {
                    continue;
                }

                var type = typeElement.GetString();
                if (string.Equals(type, "secret", StringComparison.OrdinalIgnoreCase))
                {
                    if (item.TryGetProperty("key", out var keyElement))
                    {
                        var key = keyElement.GetString();
                        if (!string.IsNullOrWhiteSpace(key))
                        {
                            tokens.Add(key);
                        }
                    }

                    continue; // intentionally skip secret value
                }

                if (item.TryGetProperty("text", out var textElement))
                {
                    var text = textElement.GetString();
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        tokens.Add(text);
                    }
                }
            }

            return string.Join(' ', tokens);
        }
    }
}
