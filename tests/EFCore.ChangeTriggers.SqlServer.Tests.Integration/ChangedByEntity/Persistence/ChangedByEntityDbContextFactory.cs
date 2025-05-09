using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Persistence
{
    public class ChangedByEntityDbContextFactory : IDesignTimeDbContextFactory<ChangedByEntityDbContext>
    {
        public ChangedByEntityDbContext CreateDbContext(string[] args)
        {
            var serviceProvider = ChangedByEntityServiceProviderBuilder.Build("Server=(localdb)\\mssqllocaldb;Database=DesignTimeDb;Trusted_Connection=True;");

            return serviceProvider.GetRequiredService<ChangedByEntityDbContext>();
        }
    }
}
