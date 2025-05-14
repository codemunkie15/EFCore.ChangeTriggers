using EFCore.ChangeTriggers.ChangeEventQueries.Infrastructure;
using EFCore.ChangeTriggers.SqlServer;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Infrastructure;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.ChangedByEntity.Configuration
{
    internal static class ChangedByEntityServiceCollectionExtensions
    {
        public static IServiceCollection AddChangedByEntity(
            this IServiceCollection serviceCollection,
            string connectionString,
            Assembly configurationsAssembly = null,
            Action<ChangeEventsDbContextOptionsBuilder> configureChangeEventQueries = null)
        {
            return serviceCollection
                .AddDbContext<ChangedByEntityDbContext>(options =>
                {
                    options
                        .UseSqlServer(connectionString, options =>
                        {
                            options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                        })
                        .UseSqlServerChangeTriggers(options =>
                        {
                            options
                                .UseChangedBy<ChangedByEntityProvider, ChangedByEntityUser>()
                                .UseChangeEventQueries(configurationsAssembly, options =>
                                {
                                    configureChangeEventQueries?.Invoke(options);
                                });
                        });
                })
                .AddScoped<EntityCurrentUserProvider>();
        }
    }
}
