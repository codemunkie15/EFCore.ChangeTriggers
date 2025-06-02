using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByScalar;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;

public class ChangedByScalarDbContext : TestDbContext<ChangedByScalarUser, ChangedByScalarUserChange>
{
    public ChangedByScalarDbContext(DbContextOptions<ChangedByScalarDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ChangedByScalarUser>(u =>
        {
            u.HasData(
                ChangedByScalarUser.SystemUser,
                new ChangedByScalarUser { Id = 2, Username = "Test User 1" },
                new ChangedByScalarUser { Id = 3, Username = "Test User 2" },
                new ChangedByScalarUser { Id = 4, Username = "Test User 3" },
                new ChangedByScalarUser { Id = 5, Username = "Test User 4" });
        });
    }
}