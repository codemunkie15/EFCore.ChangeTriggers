using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangeSourceScalar;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Configuration
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddChangeSourceScalar(this IServiceCollection services, string connectionString)
        {
            return services
                .AddSqlServerChangeTriggers<ChangeSourceScalarDbContext>(connectionString, options =>
                {
                    options.UseChangeSource<ChangeSourceScalarProvider, ChangeSource>();
                })
                .AddScoped<ScalarChangeSourceProvider>();
        }
    }
}
