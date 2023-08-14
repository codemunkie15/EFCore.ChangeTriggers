using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Configuration.ChangeTriggers;
using EFCore.ChangeTriggers.Extensions;
using EFCore.ChangeTriggers.SqlServer.Interceptors;
using EFCore.ChangeTriggers.SqlServer.Migrations;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.SqlServer
{
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Configures the DbContext to use change triggers.
        /// </summary>
        /// <typeparam name="TChangedByProvider">The type of <see cref="IChangedByProvider{TChangedBy}"/> to use.</typeparam>
        /// <typeparam name="TChangedBy">The type that <typeparamref name="TChangedByProvider"/> returns.</typeparam>
        /// <typeparam name="TChangeSourceProvider">The type of <see cref="IChangeSourceProvider{TChangeSource}"/> to use.</typeparam>
        /// <typeparam name="TChangeSource">The type that <typeparamref name="TChangeSourceProvider"/> returns.</typeparam>
        /// <param name="optionsBuilder">The builder being used to configure the context.</param>
        /// <param name="ChangeTriggersOptionsBuilder">An optional action to further configure change triggers.</param>
        /// <returns>The options builder so that further calls can be chained.</returns>
        public static DbContextOptionsBuilder UseSqlServerChangeTriggers<
            TChangedByProvider,
            TChangedBy,
            TChangeSourceProvider,
            TChangeSource>(
            this DbContextOptionsBuilder optionsBuilder,
            Action<ChangeTriggersOptions<TChangedBy, TChangeSource>>? ChangeTriggersOptionsBuilder = null)
            where TChangedByProvider : class, IChangedByProvider<TChangedBy>
            where TChangeSourceProvider : class, IChangeSourceProvider<TChangeSource>
        {
            return optionsBuilder
                .UseChangeTriggersInternal<
                    ChangeTriggersSqlServerMigrationsSqlGenerator,
                    ChangedByDbConnectionInterceptor<TChangedBy>,
                    ChangeSourceDbConnectionInterceptor<TChangeSource>,
                    TChangedByProvider,
                    TChangedBy,
                    TChangeSourceProvider,
                    TChangeSource>(ChangeTriggersOptionsBuilder);
        }

        /// <summary>
        /// Configures the DbContext to use change triggers.
        /// </summary>
        /// <param name="optionsBuilder">The builder being used to configure the context.</param>
        /// <returns>The options builder so that further configuration can be chained.</returns>
        public static DbContextOptionsBuilder UseSqlServerChangeTriggers(this DbContextOptionsBuilder optionsBuilder)
        {
            return optionsBuilder
                .UseChangeTriggersInternal<ChangeTriggersSqlServerMigrationsSqlGenerator>();
        }

        /// <summary>
        /// Configures the DbContext to use change triggers.
        /// </summary>
        /// <typeparam name="TChangedByProvider">The type of <see cref="IChangedByProvider{TChangedBy}"/> to use.</typeparam>
        /// <typeparam name="TChangedBy">The type that <typeparamref name="TChangedByProvider"/> returns.</typeparam>
        /// <param name="optionsBuilder">The builder being used to configure the context.</param>
        /// <returns>The options builder so that further configuration can be chained.</returns>
        public static DbContextOptionsBuilder UseSqlServerChangeTriggers<
            TChangedByProvider,
            TChangedBy>(this DbContextOptionsBuilder optionsBuilder,
            Action<ChangedByChangeTriggersOptions<TChangedBy>>? ChangeTriggersOptionsBuilder = null)
            where TChangedByProvider : class, IChangedByProvider<TChangedBy>
        {
            return optionsBuilder
                .UseChangeTriggersInternal<
                    ChangeTriggersSqlServerMigrationsSqlGenerator,
                    ChangedByDbConnectionInterceptor<TChangedBy>,
                    TChangedByProvider,
                    TChangedBy>();
        }

        /// <summary>
        /// Configures the DbContext to use change triggers.
        /// </summary>
        /// <typeparam name="TChangeSourceProvider">The type of <see cref="IChangeSourceProvider{TChangeSource}"/> to use.</typeparam>
        /// <typeparam name="TChangeSource">The type that <typeparamref name="TChangeSourceProvider"/> returns.</typeparam>
        /// <param name="optionsBuilder">The builder being used to configure the context.</param>
        /// <param name="ChangeTriggersOptionsBuilder">An optional action to further configure change triggers.</param>
        /// <returns>The options builder so that further configuration can be chained.</returns>
        public static DbContextOptionsBuilder UseSqlServerChangeTriggers<
            TChangeSourceProvider,
            TChangeSource>(
            this DbContextOptionsBuilder optionsBuilder,
            Action<ChangeSourceChangeTriggersOptions<TChangeSource>>? ChangeTriggersOptionsBuilder = null)
            where TChangeSourceProvider : class, IChangeSourceProvider<TChangeSource>
        {
            return optionsBuilder
                .UseChangeTriggersInternal<
                    ChangeTriggersSqlServerMigrationsSqlGenerator,
                    ChangeSourceDbConnectionInterceptor<TChangeSource>,
                    TChangeSourceProvider,
                    TChangeSource>(ChangeTriggersOptionsBuilder);
        }
    }
}