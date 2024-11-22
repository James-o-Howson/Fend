using System.Reflection;
using Fend.Data;
using Microsoft.EntityFrameworkCore;

namespace Fend.Dependencies.Data;

internal sealed class VulnerabilitiesDbContext : ModuleDbContext
{
    public VulnerabilitiesDbContext(DbContextOptions<VulnerabilitiesDbContext> options, string schema) 
        : base(options, schema)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}