using Fend.Core.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fend.Infrastructure.Data.Configuration;

public class AuditableEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> 
    where TEntity : class, IAuditableEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(v => v.CreatedBy)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(v => v.Created)
            .IsRequired();
        
        builder.Property(v => v.LastModifiedBy)
            .HasMaxLength(150);

        builder.Property(v => v.LastModified);
    }
}