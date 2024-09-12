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
            var services = new ServiceCollection()
                .AddDbContext<TestChangedByDbContext>(options =>
                {
                    options
                        .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=DesignTimeDb;Trusted_Connection=True;", options =>
                        {
                            options.MigrationsAssembly("EFCore.ChangeTriggers.SqlServer.Tests.Integration.Migrations");
                        })
                        .UseSqlServerChangeTriggers(options =>
                        {
                            options.UseChangedBy<TestChangedByProvider, TestUser>();
                        });
                })
                .AddSingleton(services => new TestCurrentUserProvider(new TestUser { Id = 1 }))
                .BuildServiceProvider();

            return services.GetRequiredService<TestChangedByDbContext>();
        }
    }
}
