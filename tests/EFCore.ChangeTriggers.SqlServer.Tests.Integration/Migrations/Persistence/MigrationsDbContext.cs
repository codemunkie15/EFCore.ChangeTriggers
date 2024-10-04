using EFCore.ChangeTriggers.Extensions;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Migrations.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

[assembly: DesignTimeServicesReference("EFCore.ChangeTriggers.ChangeTriggersDesignTimeServices, EFCore.ChangeTriggers")]
namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Migrations.Persistence;

public class MigrationsDbContext : DbContext
{
    public DbSet<MigrationsUser> TestUsers { get; set; }

    public DbSet<MigrationsUserChange> TestUserChanges { get; set; }

    public MigrationsDbContext(DbContextOptions<MigrationsDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MigrationsUser>(u =>
        {
            u.HasData(new[]
            {
                new MigrationsUser() { Id = 1, Username = "Admin 1" },
                new MigrationsUser() { Id = 2, Username = "Admin 2" },
            });
        });

        modelBuilder.Entity<MigrationsUserChange>(uc =>
        {
            uc.HasKey(uc => uc.ChangeId);
        });

        modelBuilder.AutoConfigureChangeTriggers();
    }
}
