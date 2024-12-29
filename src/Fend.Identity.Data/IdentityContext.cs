using Fend.Identity.Application.Abstractions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fend.Identity.Data;

public sealed class IdentityContext : IdentityDbContext<ApplicationUser>, IIdentityDbContext
{
    private const string Schema = "Identity";
    
    public IdentityContext(DbContextOptions<IdentityContext> options) 
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        
        base.OnModelCreating(modelBuilder);
    }
}