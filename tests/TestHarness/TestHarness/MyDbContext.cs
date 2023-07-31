﻿using EntityFrameworkCore.ChangeTrackingTriggers.Extensions;
using Microsoft.EntityFrameworkCore;
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

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(e =>
            {
                /*
                Manual configuration:
                e.HasChangeTrackingTrigger<User, UserChange, int>();
                */
                e.ToTable("Users");
                e.Property(u => u.Name).IsRequired();
                e.Property(u => u.DateOfBirth).IsRequired();
            });

            modelBuilder.Entity<UserChange>(e =>
            {
                /*
                Manual configuration:
                e.IsChangeTrackingTable<User, UserChange, int, User, ChangeSourceType>();
                */
                e.ToTable("UserChanges");
                e.Property(u => u.Name).IsRequired();
                e.Property(u => u.DateOfBirth).IsRequired();
            });

            modelBuilder.Entity<Permission>(e =>
            {
                /*
                Manual configuration:
                e.HasChangeTrackingTrigger<Permission, PermissionChange, int>();
                */
                e.ToTable("Permissions");
                e.HasKey(p => new { p.Id, p.SubId });
                e.Property(u => u.Name).IsRequired();

                e.ConfigureChangeTrackingTrigger(options =>
                {
                    options.TriggerNameFactory = tableName => $"CustomTriggerName_{tableName}";
                });
            });

            modelBuilder.Entity<PermissionChange>(e =>
            {
                /*
                Manual configuration:
                e.IsChangeTrackingTable<Permission, PermissionChange, int>();
                */
                e.ToTable("PermissionChanges");
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

            modelBuilder.AutoConfigureChangeTrackingTriggers();
        }
    }
}