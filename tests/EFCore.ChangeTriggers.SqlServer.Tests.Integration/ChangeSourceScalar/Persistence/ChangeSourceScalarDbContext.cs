using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain;
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
        modelBuilder.AutoConfigureChangeTriggers();

        modelBuilder.Entity<ChangeSourceScalarUser>(u =>
        {
            u.Property(u => u.Id).ValueGeneratedNever();

            // Seed users for migration tests
            u.HasData(
                new ChangedByEntityUser(0, "System"),
                new ChangedByEntityUser(100, "TestUser100"),
                new ChangedByEntityUser(101, "TestUser101"),
                new ChangedByEntityUser(102, "TestUser102"),
                new ChangedByEntityUser(103, "TestUser103"));
        });

        modelBuilder.Entity<ChangeSourceScalarUserChange>(uc =>
        {
            uc.HasKey(uc => uc.ChangeId);
        });
    }
}
