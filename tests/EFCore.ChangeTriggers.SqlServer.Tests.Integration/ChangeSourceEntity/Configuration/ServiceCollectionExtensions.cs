using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceEntity.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceEntity.Infrastructure;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceEntity.Persistence;
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