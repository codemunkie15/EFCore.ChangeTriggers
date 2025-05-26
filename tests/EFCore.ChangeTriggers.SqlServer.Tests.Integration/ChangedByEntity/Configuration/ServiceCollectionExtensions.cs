using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByEntity;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Configuration
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddChangedByEntity(this IServiceCollection serviceCollection, string connectionString)
        {
            return serviceCollection
                .AddSqlServerChangeTriggers<ChangedByEntityDbContext>(connectionString, options =>
                {
                    options.UseChangedBy<ChangedByEntityProvider, ChangedByEntityUser>();
                })
                // Register a service to provide the current user, to test that application dependencies are resolvable inside the EF Core service provider.
                .AddScoped<EntityCurrentUserProvider>();
        }
    }
}
