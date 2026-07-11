using SimpleWorkbench.Api.Application.Search;

namespace SimpleWorkbench.UnitTests.Application;

public class HybridSearchServiceTests
{
    [Fact]
    public void Merge_ShouldPreferHighCombinedScore()
    {
        var merged = HybridSearchService.Merge(
            lexical:
            [
                new SearchScore("n1", 0.9),
                new SearchScore("n2", 0.1)
            ],
            semantic:
            [
                new SearchScore("n1", 0.2),
                new SearchScore("n2", 0.95)
            ]);

        Assert.Equal("n1", merged.First().NoteId);
    }
}
