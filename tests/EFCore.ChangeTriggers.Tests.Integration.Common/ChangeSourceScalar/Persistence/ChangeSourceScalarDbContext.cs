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
            u.Property(u => u.Id).ValueGeneratedNever();

            // Seed users for migration tests
            u.HasData(
                new ChangeSourceScalarUser(0, "System"),
                new ChangeSourceScalarUser(100, "TestUser100"),
                new ChangeSourceScalarUser(101, "TestUser101"),
                new ChangeSourceScalarUser(102, "TestUser102"),
                new ChangeSourceScalarUser(103, "TestUser103"));
        });

        modelBuilder.Entity<ChangeSourceScalarUserChange>(uc =>
        {
            uc.HasKey(uc => uc.ChangeId);
        });
    }
}
