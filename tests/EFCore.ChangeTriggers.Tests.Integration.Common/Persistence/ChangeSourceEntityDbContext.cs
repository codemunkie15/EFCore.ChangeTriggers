using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceEntity;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;

public class ChangeSourceEntityDbContext : DbContext
{
    public DbSet<ChangeSourceEntityUser> TestUsers { get; set; }

    public DbSet<ChangeSourceEntityUserChange> TestUserChanges { get; set; }

    public DbSet<ChangeSource> ChangeSources { get; set; }

    public ChangeSourceEntityDbContext(DbContextOptions<ChangeSourceEntityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AutoConfigureChangeTriggers();

        modelBuilder.Entity<ChangeSourceEntityUser>(u =>
        {
            // Seed users for migration tests
            u.HasData(
                ChangeSourceEntityUser.SystemUser,
                new ChangeSourceEntityUser { Id = 2, Username = "Test User 1" },
                new ChangeSourceEntityUser { Id = 3, Username = "Test User 2" },
                new ChangeSourceEntityUser { Id = 4, Username = "Test User 3" },
                new ChangeSourceEntityUser { Id = 5, Username = "Test User 4" });
        });

        modelBuilder.Entity<ChangeSourceEntityUserChange>(uc =>
        {
            uc.HasKey(uc => uc.ChangeId);
        });

        modelBuilder.Entity<ChangeSource>(cs =>
        {
            cs.HasData(new[]
            {
                new { Id = 1, Name = "Migrations"},
                new { Id = 2, Name = "Tests"},
                new { Id = 3, Name = "Web API"},
                new { Id = 4, Name = "Console"},
                new { Id = 5, Name = "SQL"},
                new { Id = 6, Name = "Mobile"},
                new { Id = 7, Name = "Public API"},
                new { Id = 8, Name = "Email Service"},
                new { Id = 9, Name = "Data Retention Service"},
                new { Id = 10, Name = "Maintenance"}
            });
        });
    }
}
