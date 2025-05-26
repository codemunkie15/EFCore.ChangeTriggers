using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangeSourceEntity;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Configuration
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddChangeSourceEntity(this IServiceCollection services, string connectionString)
        {
            return services
                .AddSqlServerChangeTriggers<ChangeSourceEntityDbContext>(connectionString, options =>
                {
                    options.UseChangeSource<ChangeSourceEntityProvider, ChangeSource>();
                })
                .AddScoped<EntityChangeSourceProvider>();
        }
    }
}