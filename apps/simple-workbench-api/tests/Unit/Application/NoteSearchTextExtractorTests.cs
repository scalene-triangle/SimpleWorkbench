using SimpleWorkbench.Api.Application.Search;

namespace SimpleWorkbench.UnitTests.Application;

public class NoteSearchTextExtractorTests
{
    [Fact]
    public void Extract_ShouldExcludeSecretValuesButKeepKeys()
    {
        var json = """{"items":[{"type":"plainText","text":"db setup"},{"type":"secret","key":"DB_PASSWORD","value":"abc123"}]}""";

        var text = NoteSearchTextExtractor.Extract(json);

        Assert.Contains("db setup", text);
        Assert.Contains("DB_PASSWORD", text);
        Assert.DoesNotContain("abc123", text);
    }
}
