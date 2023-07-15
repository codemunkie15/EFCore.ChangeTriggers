using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using EntityFrameworkCore.ChangeTrackingTriggers.Configuration;
using EntityFrameworkCore.ChangeTrackingTriggers.Extensions;
using EntityFrameworkCore.ChangeTrackingTriggers.SqlServer.Interceptors;
using EntityFrameworkCore.ChangeTrackingTriggers.SqlServer.Migrations;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ChangeTrackingTriggers.SqlServer
{
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Configures the DbContext to use change tracking triggers.
        /// </summary>
        /// <typeparam name="TChangedByProvider">The type of <see cref="IChangedByProvider{TChangedBy}"/> to use.</typeparam>
        /// <typeparam name="TChangedBy">The type that <typeparamref name="TChangedByProvider"/> returns.</typeparam>
        /// <typeparam name="TChangeSourceProvider">The type of <see cref="IChangeSourceProvider{TChangeSource}"/> to use.</typeparam>
        /// <typeparam name="TChangeSource">The type that <typeparamref name="TChangeSourceProvider"/> returns.</typeparam>
        /// <param name="optionsBuilder">The builder being used to configure the context.</param>
        /// <param name="changeTrackingTriggersOptionsBuilder">An optional action to further configure change tracking triggers.</param>
        /// <returns>The options builder so that further calls can be chained.</returns>
        public static DbContextOptionsBuilder UseSqlServerChangeTrackingTriggers<
            TChangedByProvider,
            TChangedBy,
            TChangeSourceProvider,
            TChangeSource>(
            this DbContextOptionsBuilder optionsBuilder,
            Action<ChangeTrackingTriggersOptions<TChangeSource>>? changeTrackingTriggersOptionsBuilder = null)
            where TChangedByProvider : class, IChangedByProvider<TChangedBy>
            where TChangeSourceProvider : class, IChangeSourceProvider<TChangeSource>
        {
            return optionsBuilder
                .UseChangeTrackingTriggersInternal<
                    ChangeTrackingSqlServerMigrationsSqlGenerator,
                    ChangedByDbConnectionInterceptor<TChangedBy>,
                    ChangeSourceDbConnectionInterceptor<TChangeSource>,
                    TChangedByProvider,
                    TChangedBy,
                    TChangeSourceProvider,
                    TChangeSource>(changeTrackingTriggersOptionsBuilder);
        }

        /// <summary>
        /// Configures the DbContext to use change tracking triggers.
        /// </summary>
        /// <param name="optionsBuilder">The builder being used to configure the context.</param>
        /// <returns>The options builder so that further configuration can be chained.</returns>
        public static DbContextOptionsBuilder UseSqlServerChangeTrackingTriggers(this DbContextOptionsBuilder optionsBuilder)
        {
            return optionsBuilder
                .UseChangeTrackingTriggersInternal<ChangeTrackingSqlServerMigrationsSqlGenerator>();
        }

        /// <summary>
        /// Configures the DbContext to use change tracking triggers.
        /// </summary>
        /// <typeparam name="TChangedByProvider">The type of <see cref="IChangedByProvider{TChangedBy}"/> to use.</typeparam>
        /// <typeparam name="TChangedBy">The type that <typeparamref name="TChangedByProvider"/> returns.</typeparam>
        /// <param name="optionsBuilder">The builder being used to configure the context.</param>
        /// <returns>The options builder so that further configuration can be chained.</returns>
        public static DbContextOptionsBuilder UseSqlServerChangeTrackingTriggers<
            TChangedByProvider,
            TChangedBy>(this DbContextOptionsBuilder optionsBuilder)
            where TChangedByProvider : class, IChangedByProvider<TChangedBy>
        {
            return optionsBuilder
                .UseChangeTrackingTriggersInternal<
                    ChangeTrackingSqlServerMigrationsSqlGenerator,
                    ChangedByDbConnectionInterceptor<TChangedBy>,
                    TChangedByProvider,
                    TChangedBy>();
        }

        /// <summary>
        /// Configures the DbContext to use change tracking triggers.
        /// </summary>
        /// <typeparam name="TChangeSourceProvider">The type of <see cref="IChangeSourceProvider{TChangeSource}"/> to use.</typeparam>
        /// <typeparam name="TChangeSource">The type that <typeparamref name="TChangeSourceProvider"/> returns.</typeparam>
        /// <param name="optionsBuilder">The builder being used to configure the context.</param>
        /// <param name="changeTrackingTriggersOptionsBuilder">An optional action to further configure change tracking triggers.</param>
        /// <returns>The options builder so that further configuration can be chained.</returns>
        public static DbContextOptionsBuilder UseSqlServerChangeTrackingTriggers<
            TChangeSourceProvider,
            TChangeSource>(
            this DbContextOptionsBuilder optionsBuilder,
            Action<ChangeTrackingTriggersOptions<TChangeSource>>? changeTrackingTriggersOptionsBuilder = null)
            where TChangeSourceProvider : class, IChangeSourceProvider<TChangeSource>
        {
            return optionsBuilder
                .UseChangeTrackingTriggersInternal<
                    ChangeTrackingSqlServerMigrationsSqlGenerator,
                    ChangeSourceDbConnectionInterceptor<TChangeSource>,
                    TChangeSourceProvider,
                    TChangeSource>(changeTrackingTriggersOptionsBuilder);
        }
    }
}