using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Persistence
{
    public class ChangeSourceScalarDbContextFactory : IDesignTimeDbContextFactory<ChangeSourceScalarDbContext>
    {
        public ChangeSourceScalarDbContext CreateDbContext(string[] args)
        {
            var services = new ServiceCollection()
                .AddChangeSourceScalar("Server=(localdb)\\mssqllocaldb;Database=DesignTimeDb;Trusted_Connection=True;")
                .BuildServiceProvider();

            return services.GetRequiredService<ChangeSourceScalarDbContext>();
        }
    }
}
