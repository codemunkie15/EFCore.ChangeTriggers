using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceScalar.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceScalar.Infrastructure;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceScalar.Persistence;
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
