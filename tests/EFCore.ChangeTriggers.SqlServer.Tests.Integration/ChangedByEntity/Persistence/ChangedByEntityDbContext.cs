using EFCore.ChangeTriggers.Extensions;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

[assembly: DesignTimeServicesReference("EFCore.ChangeTriggers.ChangeTriggersDesignTimeServices, EFCore.ChangeTriggers")]
namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Persistence;

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
        modelBuilder.Entity<ChangedByEntityUserChange>(uc =>
        {
            uc.HasKey(uc => uc.ChangeId);
        });

        modelBuilder.AutoConfigureChangeTriggers();
    }
}
