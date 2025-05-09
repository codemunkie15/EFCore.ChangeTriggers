using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceEntity.Persistence;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Configuration
{
    public class ChangeSourceEntityDbContextFactory : IDesignTimeDbContextFactory<ChangeSourceEntityDbContext>
    {
        public ChangeSourceEntityDbContext CreateDbContext(string[] args)
        {
            var services = new ServiceCollection()
                .AddChangeSourceEntity("Server=(localdb)\\mssqllocaldb;Database=DesignTimeDb;Trusted_Connection=True;")
                .BuildServiceProvider();

            return services.GetRequiredService<ChangeSourceEntityDbContext>();
        }
    }
}