using Fend.Core.SharedKernel.Abstractions;
using Fend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fend.Identity.Data;

public interface IIdentityDbContext : IUnitOfWork
{
}

public sealed class IdentityDbContext : ModuleDbContext, IIdentityDbContext
{
    private const string Schema = "Identity";
    
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) 
        : base(options, Schema)
    {
    }
}