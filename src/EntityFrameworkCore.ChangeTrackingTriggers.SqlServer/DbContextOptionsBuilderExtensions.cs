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
        public static DbContextOptionsBuilder UseSqlServerChangeTrackingTriggers<
            TChangedByProvider,
            TChangedBy,
            TChangeSourceProvider,
            TChangeSource>(
            this DbContextOptionsBuilder optionsBuilder,
            Action<ChangeTrackingTriggersOptions<TChangeSource>> changeTrackingTriggersOptionsBuilder)
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

        public static DbContextOptionsBuilder UseSqlServerChangeTrackingTriggers(this DbContextOptionsBuilder optionsBuilder)
        {
            return optionsBuilder
                .UseChangeTrackingTriggersInternal<ChangeTrackingSqlServerMigrationsSqlGenerator>();
        }

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