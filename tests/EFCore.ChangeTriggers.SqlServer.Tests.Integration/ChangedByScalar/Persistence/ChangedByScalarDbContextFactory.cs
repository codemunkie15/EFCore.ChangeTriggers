using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Infrastructure;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Persistence
{
    public class ChangedByScalarDbContextFactory : IDesignTimeDbContextFactory<ChangedByScalarDbContext>
    {
        public ChangedByScalarDbContext CreateDbContext(string[] args)
        {
            var serviceProvider = ChangedByScalarServiceProviderBuilder.Build("Server=(localdb)\\mssqllocaldb;Database=DesignTimeDb;Trusted_Connection=True;");

            return serviceProvider.GetRequiredService<ChangedByScalarDbContext>();
        }
    }
}
