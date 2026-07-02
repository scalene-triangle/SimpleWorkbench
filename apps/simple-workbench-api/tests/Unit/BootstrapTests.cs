using Xunit;

namespace SimpleWorkbench.UnitTests;

public class BootstrapTests
{
    [Fact]
    public void ApiAssembly_ShouldLoad()
    {
        var assembly = typeof(SimpleWorkbench.Api.Program).Assembly;
        Assert.NotNull(assembly);
        Assert.Equal("SimpleWorkbench.Api", assembly.GetName().Name);
    }
}
