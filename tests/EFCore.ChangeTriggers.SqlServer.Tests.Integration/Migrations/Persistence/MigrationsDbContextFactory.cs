using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Migrations.Infrastructure;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Migrations.Persistence
{
    public class MigrationsDbContextFactory : IDesignTimeDbContextFactory<MigrationsDbContext>
    {
        public MigrationsDbContext CreateDbContext(string[] args)
        {
            var serviceProvider = MigrationsServiceProviderBuilder.Build("Server=(localdb)\\mssqllocaldb;Database=DesignTimeDb;Trusted_Connection=True;");

            return serviceProvider.GetRequiredService<MigrationsDbContext>();
        }
    }
}
