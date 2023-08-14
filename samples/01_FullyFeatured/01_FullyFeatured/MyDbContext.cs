using _01_FullyFeatured.DbModels.Orders;
using _01_FullyFeatured.DbModels.Products;
using _01_FullyFeatured.DbModels.Users;
using EFCore.ChangeTriggers.Extensions;
using Microsoft.EntityFrameworkCore;

namespace _01_FullyFeatured
{
    public class MyDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<UserChange> UserChanges { get; set; }

        public DbSet<OrderChange> OrderChanges { get; set; }

        public DbSet<ProductChange> ProductChanges { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AutoConfigureChangeTriggers();

            modelBuilder.Entity<User>(e =>
            {
                /*
                Manual configuration:
                e.HasChangeTrigger<User, UserChange>();
                */
            });

            modelBuilder.Entity<UserChange>(e =>
            {
                /*
                Manual configuration:
                e.IsChangeTable<UserChange, int, User, ChangeSourceType>();
                */
            });

            modelBuilder.Entity<Product>(e =>
            {
                /*
                Manual configuration:
                e.HasChangeTrigger<Product, ProductChange,>();
                */
            });

            modelBuilder.Entity<ProductChange>(e =>
            {
                /*
                Manual configuration:
                e.IsChangeTable<ProductChange, int>();
                */
            });

            modelBuilder.Entity<Order>(e =>
            {
                /*
                Manual configuration:
                e.HasChangeTrigger<Order, OrderChange>();
                */

                e.ConfigureChangeTrigger(options =>
                {
                    options.TriggerNameFactory = tableName => $"CustomTriggerName_{tableName}";
                });
            });

            modelBuilder.Entity<OrderChange>(e =>
            {
                /*
                Manual configuration:
                e.IsChangeTable<OrderChange, int, User, ChangeSourceType>();
                */
            });

            modelBuilder.Entity<User>(e =>
            {
                e.HasData(new User { Id = 1, Name = "Admin", DateOfBirth = "01/01/2000" });
            });
        }
    }
}