using Microsoft.EntityFrameworkCore;
using A_AspNetCore.Models.Users;
using A_AspNetCore.Models.Roles;
using EFCore.ChangeTriggers;

namespace A_AspNetCore.Data
{
    public class SampleDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserChange> UserChanges { get; set; }

        public DbSet<RoleChange> RoleChanges { get; set; }

        public SampleDbContext(DbContextOptions<SampleDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AutoConfigureChangeTriggers();

            modelBuilder.Entity<User>(e =>
            {
                e.HasData(
                [
                    new User { Id = 1, Name = "Administrator", DateOfBirth = "01/01/1970", RoleId = 1 },
                    new User { Id = 2, Name = "Migrations", DateOfBirth = "01/01/1970", RoleId = 1 },
                ]);
            });

            modelBuilder.Entity<Role>(e =>
            {
                e.HasData(new Role { Id = 1, Name = "Admin", Enabled = true });
            });

            modelBuilder.Entity<UserChange>(e =>
            {
                e.HasKey(e => e.ChangeId);
            });

            modelBuilder.Entity<RoleChange>(e =>
            {
                e.HasKey(e => e.ChangeId);
            });
        }
    }
}