using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByEntity;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.ChangedByEntity.Configuration
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddChangedByEntity(this IServiceCollection serviceCollection, string connectionString)
        {
            return serviceCollection
                .AddSqlServerChangeEventQueries<ChangedByEntityDbContext>(connectionString, options =>
                {
                    options.UseChangedBy<ChangedByEntityProvider, ChangedByEntityUser>();
                })
                .AddScoped<EntityCurrentUserProvider>();
        }
    }
}
