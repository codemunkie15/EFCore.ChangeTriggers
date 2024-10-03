using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Infrastructure;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Persistence
{
    public class TestDbContextFactory : IDesignTimeDbContextFactory<ChangedByEntityDbContext>
    {
        public ChangedByEntityDbContext CreateDbContext(string[] args)
        {
            var serviceProvider = ServiceProviderBuilder.Build("Server=(localdb)\\mssqllocaldb;Database=DesignTimeDb;Trusted_Connection=True;");

            return serviceProvider.GetRequiredService<ChangedByEntityDbContext>();
        }
    }
}
