using System.Reflection;
using Fend.Core.Domain.Dependencies;
using Fend.Dependencies.Application.Abstractions;
using Fend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fend.Dependencies.Data;

public sealed class DependenciesDbContext : ModuleDbContext, IDependenciesDbContext
{
    private const string Schema = "Dependencies";

    public DbSet<Vulnerability> Vulnerabilities { get; set; }
    public DbSet<Dependency> Dependencies { get; set; }
    public DbSet<DependencyGraph> DependencyGraphs { get; set; }
    
    public DependenciesDbContext(DbContextOptions<DependenciesDbContext> options) 
        : base(options, Schema)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}