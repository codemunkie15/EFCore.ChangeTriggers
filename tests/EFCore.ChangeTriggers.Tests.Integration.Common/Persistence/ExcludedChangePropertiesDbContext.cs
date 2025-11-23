using EFCore.ChangeTriggers.Tests.Integration.Common.Domain;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Persistence
{
    public class ExcludedChangePropertiesDbContext : TestDbContext<UserWithPassword, UserWithPasswordChange>
    {
        public ExcludedChangePropertiesDbContext(DbContextOptions<ExcludedChangePropertiesDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserWithPassword>(u =>
            {
                u.HasData(UserWithPassword.SystemUser);
            });
        }
    }
}