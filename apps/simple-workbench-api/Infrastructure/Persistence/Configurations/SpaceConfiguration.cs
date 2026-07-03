using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleWorkbench.Api.Infrastructure.Persistence.Records;

namespace SimpleWorkbench.Api.Infrastructure.Persistence.Configurations;

public sealed class SpaceConfiguration : IEntityTypeConfiguration<SpaceRecord>
{
    public void Configure(EntityTypeBuilder<SpaceRecord> builder)
    {
        builder.ToTable("Space");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasMaxLength(64);
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
    }
}
