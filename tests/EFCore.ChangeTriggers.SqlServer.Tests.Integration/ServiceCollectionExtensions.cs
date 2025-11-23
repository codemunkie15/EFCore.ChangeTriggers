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

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTestServices(this IServiceCollection serviceCollection, string connectionString)
        {
            return serviceCollection
                .AddSqlServerChangeTriggers<TestDbContext>(connectionString)
                .AddScoped<IUserReadRepository<User, UserChange>, UserReadRepository<TestDbContext, User, UserChange>>();
        }

        public static IServiceCollection AddExcludedChangePropertiesServices(this IServiceCollection serviceCollection, string connectionString)
        {
            return serviceCollection
                .AddSqlServerChangeTriggers<ExcludedChangePropertiesDbContext>(connectionString)
                .AddScoped<
                    IUserReadRepository<UserWithPassword, UserWithPasswordChange>,
                    UserReadRepository<ExcludedChangePropertiesDbContext, UserWithPassword, UserWithPasswordChange>>();
        }

        public static IServiceCollection AddChangedByEntityServices(this IServiceCollection serviceCollection, string connectionString)
        {
            return serviceCollection
                .AddSqlServerChangeTriggers<ChangedByEntityDbContext>(connectionString, options =>
                {
                    options.UseChangedBy<ChangedByEntityProvider, ChangedByEntityUser>();
                })
                // Register a service to provide the current user, to test that application dependencies are resolvable inside the EF Core service provider.
                .AddScoped<EntityCurrentUserProvider>()
                .AddScoped<IUserReadRepository<ChangedByEntityUser, ChangedByEntityUserChange>, ChangedByEntityUserReadRepository>();
        }

        public static IServiceCollection AddChangedByScalarServices(this IServiceCollection services, string connectionString)
        {
            return services
                .AddSqlServerChangeTriggers<ChangedByScalarDbContext>(connectionString, options =>
                {
                    options.UseChangedBy<ChangedByScalarProvider, string>();
                })
                // Register a service to provide the current user, to test that application dependencies are resolvable inside the EF Core service provider.
                .AddScoped<ScalarCurrentUserProvider>()
                .AddScoped<IUserReadRepository<ChangedByScalarUser, ChangedByScalarUserChange>,
                    UserReadRepository<ChangedByScalarDbContext, ChangedByScalarUser, ChangedByScalarUserChange>>();
        }

        public static IServiceCollection AddChangeSourceEntityServices(this IServiceCollection services, string connectionString)
        {
            return services
                .AddSqlServerChangeTriggers<ChangeSourceEntityDbContext>(connectionString, options =>
                {
                    options.UseChangeSource<ChangeSourceEntityProvider, ChangeSource>();
                })
                // Register a service to provide the change source, to test that application dependencies are resolvable inside the EF Core service provider.
                .AddScoped<EntityChangeSourceProvider>()
                .AddScoped<IUserReadRepository<ChangeSourceEntityUser, ChangeSourceEntityUserChange>, ChangeSourceEntityUserReadRepository>();
        }

        public static IServiceCollection AddChangeSourceScalarServices(this IServiceCollection services, string connectionString)
        {
            return services
                .AddSqlServerChangeTriggers<ChangeSourceScalarDbContext>(connectionString, options =>
                {
                    options.UseChangeSource<ChangeSourceScalarProvider, ChangeSourceType>();
                })
                // Register a service to provide the change source, to test that application dependencies are resolvable inside the EF Core service provider.
                .AddScoped<ScalarChangeSourceProvider>()
                .AddScoped<IUserReadRepository<ChangeSourceScalarUser, ChangeSourceScalarUserChange>,
                    UserReadRepository<ChangeSourceScalarDbContext, ChangeSourceScalarUser, ChangeSourceScalarUserChange>>();
        }

        private static IServiceCollection AddSqlServerChangeTriggers<TDbContext>(
            this IServiceCollection services,
            string connectionString,
            Action<ChangeTriggersSqlServerDbContextOptionsBuilder> optionsAction = null)
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
                        .UseSqlServerChangeTriggers(optionsAction);
                });
        }
    }
}