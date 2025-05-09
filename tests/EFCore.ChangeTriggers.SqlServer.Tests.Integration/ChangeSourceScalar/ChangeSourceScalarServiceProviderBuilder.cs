using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceScalar.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceScalar.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar
{
    internal static class ChangeSourceScalarServiceProviderBuilder
    {
        public static ServiceProvider Build(string connectionString)
        {
            return new ServiceCollection()
                .AddDbContext<ChangeSourceScalarDbContext>(options =>
                {
                    options
                        .UseSqlServer(connectionString)
                        .UseSqlServerChangeTriggers(options =>
                        {
                            options.UseChangeSource<ChangeSourceScalarProvider, ChangeSource>();
                        });
                })
                .AddScoped<ScalarChangeSourceProvider>()
                .BuildServiceProvider();
        }
    }
}
