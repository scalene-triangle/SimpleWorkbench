using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleWorkbench.Api.Infrastructure.Persistence.Records;

namespace SimpleWorkbench.Api.Infrastructure.Persistence.Configurations;

public sealed class InlineSecretConfiguration : IEntityTypeConfiguration<InlineSecretRecord>
{
    public void Configure(EntityTypeBuilder<InlineSecretRecord> builder)
    {
        builder.ToTable("InlineSecret");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasMaxLength(64);
        builder.Property(x => x.NoteId).HasMaxLength(64).IsRequired();
        builder.Property(x => x.SecretKey).HasMaxLength(200).IsRequired();
        builder.Property(x => x.SecretValue).HasMaxLength(4000).IsRequired();
        builder.HasIndex(x => new { x.NoteId, x.SecretKey });
    }
}
