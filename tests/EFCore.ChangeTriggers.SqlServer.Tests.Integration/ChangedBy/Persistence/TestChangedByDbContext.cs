using EFCore.ChangeTriggers.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Runtime.CompilerServices;

[assembly: DesignTimeServicesReference("EFCore.ChangeTriggers.ChangeTriggersDesignTimeServices, EFCore.ChangeTriggers")]
namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedBy.Persistence;

public class TestChangedByDbContext : DbContext
{
    public DbSet<TestUser> TestUsers { get; set; }

    public DbSet<TestUserChange> TestUserChanges { get; set; }

    public TestChangedByDbContext(DbContextOptions<TestChangedByDbContext> options)
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
        modelBuilder.Entity<TestUserChange>(uc =>
        {
            uc.HasKey(uc => uc.ChangeId);
        });

        modelBuilder.AutoConfigureChangeTriggers();
    }
}
