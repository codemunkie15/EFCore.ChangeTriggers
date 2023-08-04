﻿using _01_FullyFeatured.DbModels.Permissions;
using _01_FullyFeatured.DbModels.Users;
using EFCore.ChangeTriggers.Extensions;
using Microsoft.EntityFrameworkCore;

namespace _01_FullyFeatured
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
                e.HasChangeTrigger<User, UserChange, int>();
                */
                e.ToTable("Users");
                e.Property(u => u.Name).IsRequired();
                e.Property(u => u.DateOfBirth).IsRequired();
            });

            modelBuilder.Entity<UserChange>(e =>
            {
                /*
                Manual configuration:
                e.IsChangeTable<User, UserChange, int, User, ChangeSourceType>();
                */
                e.ToTable("UserChanges");
                e.Property(u => u.Name).IsRequired();
                e.Property(u => u.DateOfBirth).IsRequired();
            });

            modelBuilder.Entity<Permission>(e =>
            {
                /*
                Manual configuration:
                e.HasChangeTrigger<Permission, PermissionChange, int>();
                */
                e.ToTable("Permissions");
                e.Property(u => u.Name).IsRequired();

                e.ConfigureChangeTrigger(options =>
                {
                    options.TriggerNameFactory = tableName => $"CustomTriggerName_{tableName}";
                });
            });

            modelBuilder.Entity<PermissionChange>(e =>
            {
                /*
                Manual configuration:
                e.IsChangeTable<Permission, PermissionChange, int>();
                */
                e.ToTable("PermissionChanges");
                e.Property(u => u.Name).IsRequired();
            });

            modelBuilder.Entity<User>(e =>
            {
                e.HasData(new User { Id = 1, Name = "Admin", DateOfBirth = "01/01/2000" });
            });

            modelBuilder.AutoConfigureChangeTriggers();
        }
    }
}