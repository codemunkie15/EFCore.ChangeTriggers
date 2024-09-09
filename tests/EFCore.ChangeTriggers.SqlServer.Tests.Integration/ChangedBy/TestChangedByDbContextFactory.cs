using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedBy.Persistence;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using EFCore.ChangeTriggers.SqlServer.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedBy
{
    public class TestChangedByDbContextFactory : IDesignTimeDbContextFactory<TestChangedByDbContext>
    {
        public TestChangedByDbContext CreateDbContext(string[] args)
        {
            var services = new ServiceCollection();

            // Add necessary services for design-time (e.g., configuration, logging, etc.)
            services.AddDbContext<TestChangedByDbContext>(options =>
                options
                    .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=DesignTimeDb;Trusted_Connection=True;")
                    .UseSqlServerChangeTriggers(options =>
                        options.UseChangedBy<TestChangedByProvider, TestUser>()
                    ));

            services.AddScoped(services => new TestCurrentUserProvider(new TestUser { Id = 1 }));

            // Build the service provider manually for design-time purposes
            var serviceProvider = services.BuildServiceProvider();

            // Resolve the DbContext from the service provider
            return serviceProvider.GetRequiredService<TestChangedByDbContext>();
        }
    }
}
