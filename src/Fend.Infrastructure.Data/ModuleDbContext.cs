using Fend.Core.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fend.Infrastructure.Data;

public abstract class ModuleDbContext : DbContext, IUnitOfWork
{
    private readonly string _schema;
    
    protected ModuleDbContext(DbContextOptions options, string schema) : base(options)
    {
        _schema = schema;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(_schema);
        
        base.OnModelCreating(modelBuilder);
    }
}