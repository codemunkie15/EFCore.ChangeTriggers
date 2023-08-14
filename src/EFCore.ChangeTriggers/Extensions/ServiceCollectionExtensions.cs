using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Configuration.ChangeTriggers;
using EFCore.ChangeTriggers.Interceptors;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace EFCore.ChangeTriggers.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddChangedBy<
            TChangedByDbConnectionInterceptor,
            TChangedByProvider,
            TChangedBy>
            (this IServiceCollection services,
            IChangedByChangeTriggersOptions<TChangedBy> options)
            where TChangedByDbConnectionInterceptor : BaseChangedByDbConnectionInterceptor<TChangedBy>
            where TChangedByProvider : class, IChangedByProvider<TChangedBy>
        {
            return services
                .AddSingleton<IChangedByChangeTriggersOptions<TChangedBy>>(options)
                .AddScoped<IInterceptor, TChangedByDbConnectionInterceptor>()
                .AddScoped<IChangedByProvider<TChangedBy>, TChangedByProvider>();
        }

        public static IServiceCollection AddChangeSource<
            TChangeSourceDbConnectionInterceptor,
            TChangeSourceProvider,
            TChangeSource>
            (this IServiceCollection services,
            IChangeSourceChangeTriggersOptions<TChangeSource> options)
            where TChangeSourceDbConnectionInterceptor : BaseChangeSourceDbConnectionInterceptor<TChangeSource>
            where TChangeSourceProvider : class, IChangeSourceProvider<TChangeSource>
        {
            return services
                .AddSingleton<IChangeSourceChangeTriggersOptions<TChangeSource>>(options)
                .AddScoped<IInterceptor, TChangeSourceDbConnectionInterceptor>()
                .AddScoped<IChangeSourceProvider<TChangeSource>, TChangeSourceProvider>();
        }
    }
}
