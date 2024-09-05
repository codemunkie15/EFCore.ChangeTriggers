using EFCore.ChangeTriggers.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration
{
    internal class TestDbContext : DbContext
    {
        public DbSet<TestUser> TestUsers { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
        {
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
}
