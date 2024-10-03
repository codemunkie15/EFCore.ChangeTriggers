using EFCore.ChangeTriggers.Extensions;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Runtime.CompilerServices;

[assembly: DesignTimeServicesReference("EFCore.ChangeTriggers.ChangeTriggersDesignTimeServices, EFCore.ChangeTriggers")]
namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Persistence;

public class ChangedByEntityDbContext : DbContext
{
    public Guid InstanceId = Guid.NewGuid();

    public DbSet<EntityUser> TestUsers { get; set; }

    public DbSet<EntityUserChange> TestUserChanges { get; set; }

    public ChangedByEntityDbContext(DbContextOptions<ChangedByEntityDbContext> options)
        : base(options)
    {
    }

    public static string GetFilePath()
    {
        return GetFilePathInternal();
    }

    private static string GetFilePathInternal([CallerFilePath] string? path = null)
    {
        return path!;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EntityUserChange>(uc =>
        {
            uc.HasKey(uc => uc.ChangeId);
        });

        modelBuilder.AutoConfigureChangeTriggers();
    }
}
