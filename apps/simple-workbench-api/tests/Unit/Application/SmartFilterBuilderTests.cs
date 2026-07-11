using SimpleWorkbench.Api.Application.Menu;

namespace SimpleWorkbench.UnitTests.Application;

public class SmartFilterBuilderTests
{
    [Fact]
    public void Build_ShouldIncludeOnlyTagsPresentInSpace()
    {
        var notes = new[]
        {
            new NoteSnapshot(["urgent"], "P1", "todo", false),
            new NoteSnapshot(["bug"], "P2", "in-progress", true)
        };

        var filters = SmartFilterBuilder.Build(notes);

        Assert.Contains("urgent", filters.Tags);
        Assert.DoesNotContain("archived-unused", filters.Tags);
    }
}
