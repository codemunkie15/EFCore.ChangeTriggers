using EFCore.ChangeTriggers.SqlServer.Extensions;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Infrastructure
{
    internal static class ServiceProviderBuilder
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
                            options.UseChangedBy<ChangedByProvider, string>();
                        });
                })
                .AddSingleton(new CurrentUserProvider())
                .BuildServiceProvider();
        }
    }
}
