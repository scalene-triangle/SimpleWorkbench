using SimpleWorkbench.Api.Domain.Notes;
using Xunit;

namespace SimpleWorkbench.UnitTests.Domain;

public class NoteTests
{
    [Fact]
    public void CreateGlobalNote_ShouldAllowNullSpaceId()
    {
        var note = Note.CreateGlobal("n1", "Global");
        Assert.Null(note.SpaceId);
        Assert.Equal("Global", note.Title);
    }
}
