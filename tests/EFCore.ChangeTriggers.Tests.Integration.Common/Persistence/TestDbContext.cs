using EFCore.ChangeTriggers.Tests.Integration.Common.Domain;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;

public class TestDbContext : TestDbContext<User, UserChange>
{
    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(u =>
        {
            u.HasData(
                User.SystemUser,
                new User { Id = 2, Username = "Test User 1" },
                new User { Id = 3, Username = "Test User 2" },
                new User { Id = 4, Username = "Test User 3" },
                new User { Id = 5, Username = "Test User 4" });
        });
    }
}