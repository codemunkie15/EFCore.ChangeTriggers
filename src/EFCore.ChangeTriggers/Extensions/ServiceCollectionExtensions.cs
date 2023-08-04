using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Configuration;
using EFCore.ChangeTriggers.Interceptors;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.Extensions
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
            ChangeTriggersOptions<TChangeSource> options)
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
