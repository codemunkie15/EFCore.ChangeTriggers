using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Infrastructure;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Persistence
{
    public class ChangeSourceScalarDbContextFactory : IDesignTimeDbContextFactory<ChangeSourceScalarDbContext>
    {
        public ChangeSourceScalarDbContext CreateDbContext(string[] args)
        {
            var serviceProvider = ChangeSourceScalarServiceProviderBuilder.Build("Server=(localdb)\\mssqllocaldb;Database=DesignTimeDb;Trusted_Connection=True;");
            return serviceProvider.GetRequiredService<ChangeSourceScalarDbContext>();
        }
    }
}
