using EFCore.ChangeTriggers.ChangeEventQueries.Infrastructure;
using EFCore.ChangeTriggers.SqlServer;
using EFCore.ChangeTriggers.SqlServer.Infrastructure;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangeSourceEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangeSourceScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTestInfrastructure(this IServiceCollection serviceCollection, string connectionString)
        {
            return serviceCollection
                .AddSqlServerChangeEventQueries<TestDbContext>(connectionString)
                .AddScoped<IUserReadRepository<User, UserChange>, UserReadRepository<TestDbContext, User, UserChange>>();
        }

        public static IServiceCollection AddChangedByEntity(this IServiceCollection serviceCollection, string connectionString)
        {
            return serviceCollection
                .AddSqlServerChangeEventQueries<ChangedByEntityDbContext>(connectionString, options =>
                {
                    options.UseChangedBy<ChangedByEntityProvider, ChangedByEntityUser>();
                })
                .AddScoped<EntityCurrentUserProvider>()
                .AddScoped<IUserReadRepository<ChangedByEntityUser, ChangedByEntityUserChange>, ChangedByEntityUserReadRepository>();
        }

        public static IServiceCollection AddChangedByScalar(this IServiceCollection services, string connectionString)
        {
            return services
                .AddSqlServerChangeEventQueries<ChangedByScalarDbContext>(connectionString, options =>
                {
                    options.UseChangedBy<ChangedByScalarProvider, string>();
                })
                .AddScoped<ScalarCurrentUserProvider>()
                .AddScoped<IUserReadRepository<ChangedByScalarUser, ChangedByScalarUserChange>,
                    UserReadRepository<ChangedByScalarDbContext, ChangedByScalarUser, ChangedByScalarUserChange>>();
        }

        public static IServiceCollection AddChangeSourceEntity(this IServiceCollection services, string connectionString)
        {
            return services
                .AddSqlServerChangeEventQueries<ChangeSourceEntityDbContext>(connectionString, options =>
                {
                    options.UseChangeSource<ChangeSourceEntityProvider, ChangeSource>();
                })
                .AddScoped<EntityChangeSourceProvider>()
                .AddScoped<IUserReadRepository<ChangeSourceEntityUser, ChangeSourceEntityUserChange>, ChangeSourceEntityUserReadRepository>();
        }

        public static IServiceCollection AddChangeSourceScalar(this IServiceCollection services, string connectionString)
        {
            return services
                .AddSqlServerChangeEventQueries<ChangeSourceScalarDbContext>(connectionString, options =>
                {
                    options.UseChangeSource<ChangeSourceScalarProvider, ChangeSourceType>();
                })
                .AddScoped<ScalarChangeSourceProvider>()
                .AddScoped<IUserReadRepository<ChangeSourceScalarUser, ChangeSourceScalarUserChange>,
                    UserReadRepository<ChangeSourceScalarDbContext, ChangeSourceScalarUser, ChangeSourceScalarUserChange>>();
        }

        public static IServiceCollection AddSqlServerChangeEventQueries<TDbContext>(
            this IServiceCollection services,
            string connectionString,
            Action<ChangeTriggersSqlServerDbContextOptionsBuilder> optionsAction = null,
            Action<ChangeEventsDbContextOptionsBuilder> changeEventQueriesOptionsAction = null)
            where TDbContext : DbContext
        {
            return services
                .AddDbContext<TDbContext>(options =>
                {
                    options
                        .UseSqlServer(connectionString, options =>
                        {
                            options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                        })
                        .UseSqlServerChangeTriggers(options =>
                        {
                            optionsAction?.Invoke(options);

                            options.UseChangeEventQueries(changeEventQueriesOptionsAction);
                        });
                });
        }
    }
}