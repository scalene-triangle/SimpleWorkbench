namespace SimpleWorkbench.Api.Application.Secrets;

public static class InlineSecretService
{
    public static string Mask(string value) => string.IsNullOrWhiteSpace(value) ? string.Empty : "******";
}
