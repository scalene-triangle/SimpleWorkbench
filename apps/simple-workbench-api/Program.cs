var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapGet("/health", () => Results.Ok("ok"));
app.Run();

namespace SimpleWorkbench.Api
{
    public partial class Program { }
}
