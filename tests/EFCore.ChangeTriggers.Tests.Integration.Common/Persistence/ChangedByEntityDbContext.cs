using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByEntity;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;

public class ChangedByEntityDbContext : TestDbContext<ChangedByEntityUser, ChangedByEntityUserChange>
{
    public ChangedByEntityDbContext(DbContextOptions<ChangedByEntityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ChangedByEntityUser>(u =>
        {
            u.HasData(
                ChangedByEntityUser.SystemUser,
                new ChangedByEntityUser { Id = 2, Username = "Test User 1" },
                new ChangedByEntityUser { Id = 3, Username = "Test User 2" },
                new ChangedByEntityUser { Id = 4, Username = "Test User 3" },
                new ChangedByEntityUser { Id = 5, Username = "Test User 4" });
        });
    }
}