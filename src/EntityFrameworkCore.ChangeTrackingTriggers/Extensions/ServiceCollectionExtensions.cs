using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using EntityFrameworkCore.ChangeTrackingTriggers.Configuration;
using EntityFrameworkCore.ChangeTrackingTriggers.Interceptors;
using EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Migrators.Generators;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseChangedBy<
            TChangedByDbConnectionInterceptor,
            TChangedByProvider,
            TChangedByIdType>
            (this IServiceCollection services)
            where TChangedByDbConnectionInterceptor : BaseChangedByDbConnectionInterceptor<TChangedByIdType>
            where TChangedByProvider : class, IChangedByProvider<TChangedByIdType>
        {
            return services
                .AddScoped<IInterceptor, TChangedByDbConnectionInterceptor>()
                .AddScoped<IChangedByProvider<TChangedByIdType>, TChangedByProvider>()
                .AddScoped<IChangedByMigrationScriptGenerator, ChangedByMigrationScriptGenerator>();
        }

        public static IServiceCollection UseChangeSource<
            TChangeSourceDbConnectionInterceptor,
            TChangeSourceProvider,
            TChangeSource>
            (this IServiceCollection services,
            ChangeTrackingTriggersOptions<TChangeSource> options)
            where TChangeSourceDbConnectionInterceptor : BaseChangeSourceDbConnectionInterceptor<TChangeSource>
            where TChangeSourceProvider : class, IChangeSourceProvider<TChangeSource>
        {
            return services
                .AddSingleton(options) // Register options with TChangeSource
                .AddScoped<IInterceptor, TChangeSourceDbConnectionInterceptor>()
                .AddScoped<IChangeSourceProvider<TChangeSource>, TChangeSourceProvider>()
                .AddScoped<IChangeSourceMigrationScriptGenerator, ChangeSourceMigrationScriptGenerator>();
        }
    }
}
