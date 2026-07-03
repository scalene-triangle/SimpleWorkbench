using Microsoft.EntityFrameworkCore;
using SimpleWorkbench.Api.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SimpleWorkbench")
    ?? "Server=(localdb)\\mssqllocaldb;Database=SimpleWorkbench;Trusted_Connection=True;TrustServerCertificate=True";

builder.Services.AddDbContext<SimpleWorkbenchDbContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();
app.MapGet("/health", () => Results.Ok("ok"));
app.Run();

namespace SimpleWorkbench.Api
{
    public partial class Program { }
}
