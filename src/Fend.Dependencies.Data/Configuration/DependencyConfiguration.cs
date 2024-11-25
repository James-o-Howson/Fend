using Fend.Core.Domain.Dependencies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fend.Dependencies.Data.Configuration;

internal sealed class DependencyConfiguration : IEntityTypeConfiguration<Dependency> 
{
    public void Configure(EntityTypeBuilder<Dependency> builder)
    {
        builder.HasKey(d => d.Id);
        builder.OwnsOne(r => r.Id);

        builder.Property(d => d.Type)
            .IsRequired();
        
        builder.Property(d => d.Metadata)
            .HasColumnType("jsonb");

        builder.HasMany(d => d.Dependencies)
            .WithOne()
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(d => d.Dependents)
            .WithOne()
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(d => d.Vulnerabilities)
            .WithOne();
    }
}