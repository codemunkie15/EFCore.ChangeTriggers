using EFCore.ChangeTriggers.Tests.Integration.Common.Basic.Domain;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Basic.Persistence;

public class BasicDbContext : DbContext
{
    public DbSet<User> TestUsers { get; set; }

    public DbSet<UserChange> TestUserChanges { get; set; }

    public BasicDbContext(DbContextOptions<BasicDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AutoConfigureChangeTriggers();

        modelBuilder.Entity<User>(u =>
        {
            u.HasData(
                User.SystemUser);
        });

        modelBuilder.Entity<UserChange>(uc =>
        {
            uc.HasKey(uc => uc.ChangeId);
        });
    }
}
