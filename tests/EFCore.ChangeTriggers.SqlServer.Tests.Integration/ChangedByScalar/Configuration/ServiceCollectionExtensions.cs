using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByScalar.Infrastructure;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByScalar.Persistence;
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
