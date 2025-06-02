using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceScalar;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;

public class ChangeSourceScalarDbContext : TestDbContext<ChangeSourceScalarUser, ChangeSourceScalarUserChange>
{
    public ChangeSourceScalarDbContext(DbContextOptions<ChangeSourceScalarDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ChangeSourceScalarUser>(u =>
        {
            u.HasData(
                ChangeSourceScalarUser.SystemUser,
                new ChangeSourceScalarUser { Id = 2, Username = "Test User 1" },
                new ChangeSourceScalarUser { Id = 3, Username = "Test User 2" },
                new ChangeSourceScalarUser { Id = 4, Username = "Test User 3" },
                new ChangeSourceScalarUser { Id = 5, Username = "Test User 4" });
        });
    }
}