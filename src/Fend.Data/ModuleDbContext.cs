using Microsoft.EntityFrameworkCore;

namespace Fend.Data;

public abstract class ModuleDbContext : DbContext
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