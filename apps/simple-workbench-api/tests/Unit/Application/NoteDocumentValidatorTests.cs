using SimpleWorkbench.Api.Application.Notes.Validation;

namespace SimpleWorkbench.UnitTests.Application;

public class NoteDocumentValidatorTests
{
    [Fact]
    public void Validator_ShouldRejectUnknownItemType()
    {
        var json = """{"items":[{"id":"i1","type":"unknown"}]}""";

        var result = NoteDocumentValidator.Validate(json);

        Assert.False(result.IsValid);
        Assert.Contains("unknown", result.Errors.Single());
    }
}
