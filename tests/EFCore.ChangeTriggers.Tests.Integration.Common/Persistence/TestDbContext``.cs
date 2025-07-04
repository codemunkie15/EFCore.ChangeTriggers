using Microsoft.EntityFrameworkCore;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;

public class TestDbContext<TUser, TUserChange> : DbContext
    where TUser : UserBase
    where TUserChange : class, IHasChangeId
{
    public DbSet<TUser> TestUsers { get; set; }

    public DbSet<TUserChange> TestUserChanges { get; set; }

    public TestDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AutoConfigureChangeTriggers();

        modelBuilder.Entity<TUserChange>().HasKey(x => x.ChangeId);
    }
}