using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SimpleWorkbench.Api.Infrastructure.Persistence;

public sealed class SimpleWorkbenchDesignTimeDbContextFactory : IDesignTimeDbContextFactory<SimpleWorkbenchDbContext>
{
    public SimpleWorkbenchDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SimpleWorkbenchDbContext>();
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\mssqllocaldb;Database=SimpleWorkbenchDesign;Trusted_Connection=True;TrustServerCertificate=True");

        return new SimpleWorkbenchDbContext(optionsBuilder.Options);
    }
}
