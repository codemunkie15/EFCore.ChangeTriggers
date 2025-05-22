using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceEntity.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceScalar.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceScalar.Persistence;

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
            // Seed users for migration tests
            u.HasData(
                ChangeSourceScalarUser.SystemUser,
                new ChangeSourceScalarUser { Id = 2, Username = "Test User 1" },
                new ChangeSourceScalarUser { Id = 3, Username = "Test User 2" },
                new ChangeSourceScalarUser { Id = 4, Username = "Test User 3" },
                new ChangeSourceScalarUser { Id = 5, Username = "Test User 4" });
        });

        modelBuilder.Entity<ChangeSourceScalarUserChange>(uc =>
        {
            uc.HasKey(uc => uc.ChangeId);
        });
    }
}
