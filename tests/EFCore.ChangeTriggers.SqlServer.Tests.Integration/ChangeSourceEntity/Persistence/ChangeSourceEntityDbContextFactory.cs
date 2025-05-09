using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Persistence
{
    public class ChangeSourceEntityDbContextFactory : IDesignTimeDbContextFactory<ChangeSourceEntityDbContext>
    {
        public ChangeSourceEntityDbContext CreateDbContext(string[] args)
        {
            var serviceProvider = ChangeSourceEntityServiceProviderBuilder.Build("Server=(localdb)\\mssqllocaldb;Database=DesignTimeDb;Trusted_Connection=True;");
            return serviceProvider.GetRequiredService<ChangeSourceEntityDbContext>();
        }
    }
}
