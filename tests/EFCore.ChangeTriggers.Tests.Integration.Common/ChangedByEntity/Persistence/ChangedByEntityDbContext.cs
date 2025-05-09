using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Persistence;

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
            u.Property(u => u.Id).ValueGeneratedNever();

            // Seed users for migration tests
            u.HasData(
                new ChangedByEntityUser(0, "System"),
                new ChangedByEntityUser(100, "TestUser100"),
                new ChangedByEntityUser(101, "TestUser101"),
                new ChangedByEntityUser(102, "TestUser102"),
                new ChangedByEntityUser(103, "TestUser103"));
        });

        modelBuilder.Entity<ChangedByEntityUserChange>(uc =>
        {
            uc.HasKey(uc => uc.ChangeId);
        });
    }
}
