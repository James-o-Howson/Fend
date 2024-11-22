using Fend.Data;
using Microsoft.EntityFrameworkCore;

namespace Fend.Identity.Data;

public sealed class IdentityDbContext : ModuleDbContext
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options, string schema) : base(options, schema)
    {
    }
}