using EFCore.ChangeTriggers.Tests.Integration.Common.Domain;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;

public class TestDbContext : DbContext
{
    public DbSet<User> TestUsers { get; set; }

    public DbSet<UserChange> TestUserChanges { get; set; }

    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AutoConfigureChangeTriggers();

        modelBuilder.Entity<User>(u =>
        {
            // Seed users for migration tests
            u.HasData(
                User.SystemUser,
                new User { Id = 2, Username = "Test User 1" },
                new User { Id = 3, Username = "Test User 2" },
                new User { Id = 4, Username = "Test User 3" },
                new User { Id = 5, Username = "Test User 4" });
        });

        modelBuilder.Entity<UserChange>(uc =>
        {
            uc.HasKey(uc => uc.ChangeId);
        });
    }
}
