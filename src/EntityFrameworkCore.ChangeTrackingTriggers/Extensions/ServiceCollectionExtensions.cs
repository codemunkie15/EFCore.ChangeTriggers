using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using EntityFrameworkCore.ChangeTrackingTriggers.Configuration;
using EntityFrameworkCore.ChangeTrackingTriggers.Interceptors;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddChangedBy<
            TChangedByDbConnectionInterceptor,
            TChangedByProvider,
            TChangedBy>
            (this IServiceCollection services)
            where TChangedByDbConnectionInterceptor : BaseChangedByDbConnectionInterceptor<TChangedBy>
            where TChangedByProvider : class, IChangedByProvider<TChangedBy>
        {
            return services
                .AddScoped<IInterceptor, TChangedByDbConnectionInterceptor>()
                .AddScoped<IChangedByProvider<TChangedBy>, TChangedByProvider>();
        }

        public static IServiceCollection AddChangeSource<
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
                .AddScoped<IChangeSourceProvider<TChangeSource>, TChangeSourceProvider>();
        }
    }
}
