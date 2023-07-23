using EntityFrameworkCore.ChangeTrackingTriggers.EfCoreExtension;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using EntityFrameworkCore.ChangeTrackingTriggers.Migrations;
using Microsoft.Extensions.DependencyInjection;
using EntityFrameworkCore.ChangeTrackingTriggers.Interceptors;
using EntityFrameworkCore.ChangeTrackingTriggers.Configuration;
using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Migrators;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Extensions
{
    internal static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseChangeTrackingTriggersInternal<
            TMigrationsSqlGenerator,
            TChangedByDbConnectionInterceptor,
            TChangeSourceDbConnectionInterceptor,
            TChangedByProvider,
            TChangedBy,
            TChangeSourceProvider,
            TChangeSource>(
            this DbContextOptionsBuilder builder,
            Action<ChangeTrackingTriggersOptions<TChangeSource>>? optionsBuilder = null)
            where TMigrationsSqlGenerator: IMigrationsSqlGenerator
            where TChangedByDbConnectionInterceptor : BaseChangedByDbConnectionInterceptor<TChangedBy>
            where TChangeSourceDbConnectionInterceptor : BaseChangeSourceDbConnectionInterceptor<TChangeSource>
            where TChangedByProvider: class, IChangedByProvider<TChangedBy>
            where TChangeSourceProvider: class, IChangeSourceProvider<TChangeSource>
        {
            var options = ChangeTrackingTriggersOptions<TChangeSource>.Create(optionsBuilder);

            return builder
                .AddChangeTrackingExtension<TMigrationsSqlGenerator, ChangeTrackingTriggersMigrator<TChangedBy, TChangeSource>>(options, services =>
                {
                    services
                        .AddChangedBy<TChangedByDbConnectionInterceptor, TChangedByProvider, TChangedBy>()
                        .AddChangeSource<TChangeSourceDbConnectionInterceptor, TChangeSourceProvider, TChangeSource>(options);
                });
        }

        public static DbContextOptionsBuilder UseChangeTrackingTriggersInternal<TMigrationsSqlGenerator>(
            this DbContextOptionsBuilder builder,
            Action<ChangeTrackingTriggersOptions>? optionsBuilder = null)
            where TMigrationsSqlGenerator : IMigrationsSqlGenerator
        {
            var options = ChangeTrackingTriggersOptions.Create(optionsBuilder);

            return builder
                .AddChangeTrackingExtension<TMigrationsSqlGenerator>(options);
        }

        public static DbContextOptionsBuilder UseChangeTrackingTriggersInternal<
            TMigrationsSqlGenerator,
            TChangedByDbConnectionInterceptor,
            TChangedByProvider,
            TChangedBy>(
            this DbContextOptionsBuilder builder,
            Action<ChangeTrackingTriggersOptions>? optionsBuilder = null)
            where TMigrationsSqlGenerator : IMigrationsSqlGenerator
            where TChangedByDbConnectionInterceptor : BaseChangedByDbConnectionInterceptor<TChangedBy>
            where TChangedByProvider : class, IChangedByProvider<TChangedBy>
        {
            var options = ChangeTrackingTriggersOptions.Create(optionsBuilder);

            return builder
                .AddChangeTrackingExtension<TMigrationsSqlGenerator, ChangeTrackingTriggersChangedByMigrator<TChangedBy>>(options, services =>
                {
                    services
                        .AddChangedBy<TChangedByDbConnectionInterceptor, TChangedByProvider, TChangedBy>();
                });
        }

        public static DbContextOptionsBuilder UseChangeTrackingTriggersInternal<
            TMigrationsSqlGenerator,
            TChangeSourceDbConnectionInterceptor,
            TChangeSourceProvider,
            TChangeSource>(
            this DbContextOptionsBuilder builder,
            Action<ChangeTrackingTriggersOptions<TChangeSource>>? optionsBuilder = null)
            where TMigrationsSqlGenerator : IMigrationsSqlGenerator
            where TChangeSourceDbConnectionInterceptor : BaseChangeSourceDbConnectionInterceptor<TChangeSource>
            where TChangeSourceProvider : class, IChangeSourceProvider<TChangeSource>
        {
            var options = ChangeTrackingTriggersOptions<TChangeSource>.Create(optionsBuilder);

            return builder
                .AddChangeTrackingExtension<TMigrationsSqlGenerator, ChangeTrackingTriggersChangeSourceMigrator<TChangeSource>>(options, services =>
                {
                    services
                        .AddChangeSource<TChangeSourceDbConnectionInterceptor, TChangeSourceProvider, TChangeSource>(options);
                });
        }

        private static DbContextOptionsBuilder AddChangeTrackingExtension<TMigrationsSqlGenerator>(
            this DbContextOptionsBuilder builder,
            ChangeTrackingTriggersOptions options,
            Action<IServiceCollection>? servicesBuilder = null)
            where TMigrationsSqlGenerator : IMigrationsSqlGenerator
        {
            var builderInfrastructure = (IDbContextOptionsBuilderInfrastructure)builder;

            builderInfrastructure
                .AddOrUpdateExtension(new ChangeTrackingExtension(services =>
                {
                    services.AddSingleton(options);

                    servicesBuilder?.Invoke(services);
                }));

            return builder
                .ReplaceService<IMigrationsModelDiffer, ChangeTrackingTriggersMigrationsModelDiffer>()
                .ReplaceService<IMigrationsSqlGenerator, TMigrationsSqlGenerator>();
        }

        private static DbContextOptionsBuilder AddChangeTrackingExtension<TMigrationsSqlGenerator, TMigrator>(
            this DbContextOptionsBuilder builder,
            ChangeTrackingTriggersOptions options,
            Action<IServiceCollection>? servicesBuilder = null)
            where TMigrationsSqlGenerator : IMigrationsSqlGenerator
            where TMigrator : IMigrator
        {
            return builder
                .AddChangeTrackingExtension<TMigrationsSqlGenerator>(options, servicesBuilder)
                .ReplaceService<IMigrator, TMigrator>();
        }
    }
}