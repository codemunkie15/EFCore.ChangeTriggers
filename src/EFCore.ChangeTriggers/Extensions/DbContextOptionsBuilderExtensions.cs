using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using EFCore.ChangeTriggers.Migrations.Migrators;
using EFCore.ChangeTriggers.Interceptors;
using EFCore.ChangeTriggers.Configuration;
using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Migrations;
using EFCore.ChangeTriggers.EfCoreExtension;

namespace EFCore.ChangeTriggers.Extensions
{
    internal static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseChangeTriggersInternal<
            TMigrationsSqlGenerator,
            TChangedByDbConnectionInterceptor,
            TChangeSourceDbConnectionInterceptor,
            TChangedByProvider,
            TChangedBy,
            TChangeSourceProvider,
            TChangeSource>(
            this DbContextOptionsBuilder builder,
            Action<ChangeTriggersOptions<TChangeSource>>? optionsBuilder = null)
            where TMigrationsSqlGenerator : IMigrationsSqlGenerator
            where TChangedByDbConnectionInterceptor : BaseChangedByDbConnectionInterceptor<TChangedBy>
            where TChangeSourceDbConnectionInterceptor : BaseChangeSourceDbConnectionInterceptor<TChangeSource>
            where TChangedByProvider : class, IChangedByProvider<TChangedBy>
            where TChangeSourceProvider : class, IChangeSourceProvider<TChangeSource>
        {
            var options = ChangeTriggersOptions<TChangeSource>.Create(optionsBuilder);

            return builder
                .AddChangeTriggersExtension<TMigrationsSqlGenerator, ChangeTriggersMigrator<TChangedBy, TChangeSource>>(options, services =>
                {
                    services
                        .AddChangedBy<TChangedByDbConnectionInterceptor, TChangedByProvider, TChangedBy>()
                        .AddChangeSource<TChangeSourceDbConnectionInterceptor, TChangeSourceProvider, TChangeSource>(options);
                });
        }

        public static DbContextOptionsBuilder UseChangeTriggersInternal<TMigrationsSqlGenerator>(
            this DbContextOptionsBuilder builder,
            Action<ChangeTriggersOptions>? optionsBuilder = null)
            where TMigrationsSqlGenerator : IMigrationsSqlGenerator
        {
            var options = ChangeTriggersOptions.Create(optionsBuilder);

            return builder
                .AddChangeTriggersExtension<TMigrationsSqlGenerator>(options);
        }

        public static DbContextOptionsBuilder UseChangeTriggersInternal<
            TMigrationsSqlGenerator,
            TChangedByDbConnectionInterceptor,
            TChangedByProvider,
            TChangedBy>(
            this DbContextOptionsBuilder builder,
            Action<ChangeTriggersOptions>? optionsBuilder = null)
            where TMigrationsSqlGenerator : IMigrationsSqlGenerator
            where TChangedByDbConnectionInterceptor : BaseChangedByDbConnectionInterceptor<TChangedBy>
            where TChangedByProvider : class, IChangedByProvider<TChangedBy>
        {
            var options = ChangeTriggersOptions.Create(optionsBuilder);

            return builder
                .AddChangeTriggersExtension<TMigrationsSqlGenerator, ChangeTriggersChangedByMigrator<TChangedBy>>(options, services =>
                {
                    services
                        .AddChangedBy<TChangedByDbConnectionInterceptor, TChangedByProvider, TChangedBy>();
                });
        }

        public static DbContextOptionsBuilder UseChangeTriggersInternal<
            TMigrationsSqlGenerator,
            TChangeSourceDbConnectionInterceptor,
            TChangeSourceProvider,
            TChangeSource>(
            this DbContextOptionsBuilder builder,
            Action<ChangeTriggersOptions<TChangeSource>>? optionsBuilder = null)
            where TMigrationsSqlGenerator : IMigrationsSqlGenerator
            where TChangeSourceDbConnectionInterceptor : BaseChangeSourceDbConnectionInterceptor<TChangeSource>
            where TChangeSourceProvider : class, IChangeSourceProvider<TChangeSource>
        {
            var options = ChangeTriggersOptions<TChangeSource>.Create(optionsBuilder);

            return builder
                .AddChangeTriggersExtension<TMigrationsSqlGenerator, ChangeTriggersChangeSourceMigrator<TChangeSource>>(options, services =>
                {
                    services
                        .AddChangeSource<TChangeSourceDbConnectionInterceptor, TChangeSourceProvider, TChangeSource>(options);
                });
        }

        private static DbContextOptionsBuilder AddChangeTriggersExtension<TMigrationsSqlGenerator>(
            this DbContextOptionsBuilder builder,
            ChangeTriggersOptions options,
            Action<IServiceCollection>? servicesBuilder = null)
            where TMigrationsSqlGenerator : IMigrationsSqlGenerator
        {
            var builderInfrastructure = (IDbContextOptionsBuilderInfrastructure)builder;

            builderInfrastructure
                .AddOrUpdateExtension(new ChangeTriggersExtension(services =>
                {
                    services.AddSingleton(options);

                    servicesBuilder?.Invoke(services);
                }));

            return builder
                .ReplaceService<IMigrationsModelDiffer, ChangeTriggersMigrationsModelDiffer>()
                .ReplaceService<IMigrationsSqlGenerator, TMigrationsSqlGenerator>();
        }

        private static DbContextOptionsBuilder AddChangeTriggersExtension<TMigrationsSqlGenerator, TMigrator>(
            this DbContextOptionsBuilder builder,
            ChangeTriggersOptions options,
            Action<IServiceCollection>? servicesBuilder = null)
            where TMigrationsSqlGenerator : IMigrationsSqlGenerator
            where TMigrator : IMigrator
        {
            return builder
                .AddChangeTriggersExtension<TMigrationsSqlGenerator>(options, servicesBuilder)
                .ReplaceService<IMigrator, TMigrator>();
        }
    }
}