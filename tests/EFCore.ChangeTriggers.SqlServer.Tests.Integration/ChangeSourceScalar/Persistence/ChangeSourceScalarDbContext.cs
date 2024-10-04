using EFCore.ChangeTriggers.Extensions;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

[assembly: DesignTimeServicesReference("EFCore.ChangeTriggers.ChangeTriggersDesignTimeServices, EFCore.ChangeTriggers")]
namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Persistence;

public class ChangeSourceScalarDbContext : DbContext
{
    public DbSet<ChangeSourceScalarUser> TestUsers { get; set; }

    public DbSet<ChangeSourceScalarUserChange> TestUserChanges { get; set; }

    public ChangeSourceScalarDbContext(DbContextOptions<ChangeSourceScalarDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChangeSourceScalarUserChange>(uc =>
        {
            uc.HasKey(uc => uc.ChangeId);
        });

        modelBuilder.AutoConfigureChangeTriggers();
    }
}
