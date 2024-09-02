using EFCore.ChangeTriggers.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TestHarness.DbModels.PaymentMethods;
using TestHarness.DbModels.Permissions;
using TestHarness.DbModels.Users;

namespace TestHarness
{
    public class MyDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<UserChange> UserChanges { get; set; }

        public DbSet<PermissionChange> PermissionChanges { get; set; }

        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("Users");
                e.Property(u => u.Name).IsRequired();
                e.Property(u => u.DateOfBirth).IsRequired();
                e.HasMany(u => u.PaymentMethods).WithOne(pm => pm.User);
            });

            modelBuilder.Entity<UserChange>(e =>
            {
                e.ToTable("UserChanges");
                e.HasKey(u => u.ChangeId);
                e.Property(u => u.Name).IsRequired();
                e.Property(u => u.DateOfBirth).IsRequired();
            });

            modelBuilder.Entity<Permission>(e =>
            {
                e.ToTable("Permissions");
                e.HasKey(p => new { p.Id, p.SubId });
                e.Property(u => u.Name).IsRequired();
            });

            modelBuilder.Entity<PermissionChange>(e =>
            {
                e.ToTable("PermissionChanges");
                e.HasKey(p => p.ChangeId);
                e.Property(u => u.Name).IsRequired();
            });

            modelBuilder.Entity<User>(e =>
            {
                e.HasData(new User { Id = 1, Name = "Admin", DateOfBirth = "01/01/2000" });
            });

            modelBuilder.Entity<Permission>(p =>
            {
                p.HasData(new Permission { Id = 1, SubId = 1, Name = "Permission 1" });
            });

            modelBuilder.AutoConfigureChangeTriggers();
        }
    }
}