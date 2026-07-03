using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleWorkbench.Api.Infrastructure.Persistence.Records;

namespace SimpleWorkbench.Api.Infrastructure.Persistence.Configurations;

public sealed class NoteConfiguration : IEntityTypeConfiguration<NoteRecord>
{
    public void Configure(EntityTypeBuilder<NoteRecord> builder)
    {
        builder.ToTable("Note");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasMaxLength(64);
        builder.Property(x => x.Title).HasMaxLength(300).IsRequired();
        builder.Property(x => x.SpaceId).HasMaxLength(64).IsRequired(false);
    }
}
