using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByEntity;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;

public class ChangedByEntityDbContext : DbContext
{
    public DbSet<ChangedByEntityUser> TestUsers { get; set; }

    public DbSet<ChangedByEntityUserChange> TestUserChanges { get; set; }

    public ChangedByEntityDbContext(DbContextOptions<ChangedByEntityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AutoConfigureChangeTriggers();

        modelBuilder.Entity<ChangedByEntityUser>(u =>
        {
            // Seed users for migration tests
            u.HasData(
                ChangedByEntityUser.SystemUser,
                new ChangedByEntityUser { Id = 2, Username = "Test User 1" },
                new ChangedByEntityUser { Id = 3, Username = "Test User 2" },
                new ChangedByEntityUser { Id = 4, Username = "Test User 3" },
                new ChangedByEntityUser { Id = 5, Username = "Test User 4" });
        });

        modelBuilder.Entity<ChangedByEntityUserChange>(uc =>
        {
            uc.HasKey(uc => uc.ChangeId);
        });
    }
}
