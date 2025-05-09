using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Persistence
{
    public class ChangedByScalarDbContextFactory : IDesignTimeDbContextFactory<ChangedByScalarDbContext>
    {
        public ChangedByScalarDbContext CreateDbContext(string[] args)
        {
            var services = new ServiceCollection()
                .AddChangedByScalar("Server=(localdb)\\mssqllocaldb;Database=DesignTimeDb;Trusted_Connection=True;")
                .BuildServiceProvider();

            return services.GetRequiredService<ChangedByScalarDbContext>();
        }
    }
}
