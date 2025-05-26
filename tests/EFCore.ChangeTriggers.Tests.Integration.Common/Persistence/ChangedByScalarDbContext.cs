using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByScalar;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;

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
            // Seed users for migration tests
            u.HasData(
                ChangedByScalarUser.SystemUser,
                new ChangedByScalarUser { Id = 2, Username = "Test User 1" },
                new ChangedByScalarUser { Id = 3, Username = "Test User 2" },
                new ChangedByScalarUser { Id = 4, Username = "Test User 3" },
                new ChangedByScalarUser { Id = 5, Username = "Test User 4" });
        });

        modelBuilder.Entity<ChangedByScalarUserChange>(uc =>
        {
            uc.HasKey(uc => uc.ChangeId);
        });
    }
}
