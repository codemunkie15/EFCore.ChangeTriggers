using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByScalar;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Configuration
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddChangedByScalar(this IServiceCollection services, string connectionString)
        {
            return services
                .AddSqlServerChangeTriggers<ChangedByScalarDbContext>(connectionString, options =>
                {
                    options.UseChangedBy<ChangedByScalarProvider, string>();
                })
                .AddScoped<ScalarCurrentUserProvider>();
        }
    }
}
