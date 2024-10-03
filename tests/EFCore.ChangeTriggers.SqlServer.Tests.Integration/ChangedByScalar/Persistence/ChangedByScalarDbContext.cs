using EFCore.ChangeTriggers.Extensions;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

[assembly: DesignTimeServicesReference("EFCore.ChangeTriggers.ChangeTriggersDesignTimeServices, EFCore.ChangeTriggers")]
namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Persistence;

public class ChangedByScalarDbContext : DbContext
{
    public Guid InstanceId = Guid.NewGuid();

    public DbSet<ScalarUser> TestUsers { get; set; }

    public DbSet<ScalarUserChange> TestUserChanges { get; set; }

    public ChangedByScalarDbContext(DbContextOptions<ChangedByScalarDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ScalarUserChange>(uc =>
        {
            uc.HasKey(uc => uc.ChangeId);
        });

        modelBuilder.AutoConfigureChangeTriggers();
    }
}
