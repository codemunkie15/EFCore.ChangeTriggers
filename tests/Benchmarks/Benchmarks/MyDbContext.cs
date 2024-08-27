using Benchmarks.DbModels.Users;
using EFCore.ChangeTriggers.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Benchmarks
{
    public class MyDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<UserChange> UserChanges { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AutoConfigureChangeTriggers();

            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("Users");
            });

            modelBuilder.Entity<UserChange>(e =>
            {
                e.ToTable("UserChanges");
            });
        }
    }
}