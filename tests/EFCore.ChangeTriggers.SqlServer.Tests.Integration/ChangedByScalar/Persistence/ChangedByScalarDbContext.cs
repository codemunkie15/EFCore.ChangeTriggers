using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByScalar.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

[assembly: DesignTimeServicesReference("EFCore.ChangeTriggers.ChangeTriggersDesignTimeServices, EFCore.ChangeTriggers")]
namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Persistence;

public class ChangedByScalarDbContext : DbContext
{
    public DbSet<ChangedByScalarUser> TestUsers { get; set; }

    public DbSet<ChangedByScalarUserChange> TestUserChanges { get; set; }

    public ChangedByScalarDbContext(DbContextOptions<ChangedByScalarDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AutoConfigureChangeTriggers();

        modelBuilder.Entity<ChangedByScalarUser>(u =>
        {
            u.Property(u => u.Id).ValueGeneratedNever();

            // Seed users for migration tests
            u.HasData(
                new ChangedByScalarUser(0, "System"),
                new ChangedByScalarUser(100, "TestUser100"),
                new ChangedByScalarUser(101, "TestUser101"),
                new ChangedByScalarUser(102, "TestUser102"),
                new ChangedByScalarUser(103, "TestUser103"));
        });

        modelBuilder.Entity<ChangedByScalarUserChange>(uc =>
        {
            uc.HasKey(uc => uc.ChangeId);
        });
    }
}
