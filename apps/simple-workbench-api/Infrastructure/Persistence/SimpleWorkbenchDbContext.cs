using Microsoft.EntityFrameworkCore;
using SimpleWorkbench.Api.Infrastructure.Persistence.Configurations;
using SimpleWorkbench.Api.Infrastructure.Persistence.Records;

namespace SimpleWorkbench.Api.Infrastructure.Persistence;

public sealed class SimpleWorkbenchDbContext(DbContextOptions<SimpleWorkbenchDbContext> options) : DbContext(options)
{
    public DbSet<NoteRecord> Notes => Set<NoteRecord>();
    public DbSet<SpaceRecord> Spaces => Set<SpaceRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new NoteConfiguration());
        modelBuilder.ApplyConfiguration(new SpaceConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
