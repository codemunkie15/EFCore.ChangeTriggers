using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Infrastructure
{
    internal static class ChangedByScalarServiceProviderBuilder
    {
        public static ServiceProvider Build(string connectionString)
        {
            return new ServiceCollection()
                .AddDbContext<ChangedByScalarDbContext>(options =>
                {
                    options
                        .UseSqlServer(connectionString)
                        .UseSqlServerChangeTriggers(options =>
                        {
                            options.UseChangedBy<ChangedByScalarProvider, string>();
                        });
                })
                .AddScoped<ScalarCurrentUserProvider>()
                .BuildServiceProvider();
        }
    }
}
