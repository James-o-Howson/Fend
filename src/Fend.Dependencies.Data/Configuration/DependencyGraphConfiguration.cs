using Fend.Core.Domain.Dependencies;
using Fend.Core.Domain.Dependencies.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fend.Dependencies.Data.Configuration;

internal sealed class DependencyGraphConfiguration : IEntityTypeConfiguration<DependencyGraph> 
{
    public void Configure(EntityTypeBuilder<DependencyGraph> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(r => r.Id)
            .HasConversion(id => id.Value,
                value => DependencyGraphId.Explicit(value))
            .ValueGeneratedNever();

        builder.HasOne(d => d.Root)
            .WithMany()
            .IsRequired();
    }
}