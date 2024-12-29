using System.Reflection;
using Fend.Core.Domain.Dependencies;
using Fend.Dependencies.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fend.Dependencies.Data;

public sealed class DependenciesDbContext : DbContext, IDependenciesDbContext
{
    private const string Schema = "Dependencies";

    public DbSet<Vulnerability> Vulnerabilities { get; set; }
    public DbSet<Dependency> Dependencies { get; set; }
    public DbSet<DependencyGraph> DependencyGraphs { get; set; }
    
    public DependenciesDbContext(DbContextOptions<DependenciesDbContext> options) 
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
    }
}