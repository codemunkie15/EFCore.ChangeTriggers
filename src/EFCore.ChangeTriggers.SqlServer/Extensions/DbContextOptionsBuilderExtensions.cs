using EFCore.ChangeTriggers.Extensions;
using EFCore.ChangeTriggers.SqlServer.Infrastructure;
using EFCore.ChangeTriggers.SqlServer.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore.ChangeTriggers.SqlServer.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Configures the DbContext to use change triggers.
        /// </summary>
        /// <param name="optionsBuilder">The builder being used to configure the context.</param>
        /// <returns>The options builder so that further configuration can be chained.</returns>
        public static DbContextOptionsBuilder UseSqlServerChangeTriggers(
            this DbContextOptionsBuilder optionsBuilder,
            Action<ChangeTriggersSqlServerDbContextOptionsBuilder>? changeTriggersOptionsAction = null)
        {
            var extension = optionsBuilder.GetOrCreateExtension<ChangeTriggersSqlServerDbContextOptionsExtension>();
            optionsBuilder.AsInfrastructure().AddOrUpdateExtension(extension);

            return optionsBuilder
                .UseChangeTriggers()
                .ApplyConfiguration(changeTriggersOptionsAction)
                .ReplaceService<IMigrationsSqlGenerator, ChangeTriggersSqlServerMigrationsSqlGenerator>();
        }

        private static DbContextOptionsBuilder ApplyConfiguration(this DbContextOptionsBuilder optionsBuilder,
            Action<ChangeTriggersSqlServerDbContextOptionsBuilder>? changeTriggersOptionsAction)
        {
            changeTriggersOptionsAction?.Invoke(new ChangeTriggersSqlServerDbContextOptionsBuilder(optionsBuilder));

            var extension = optionsBuilder.GetOrCreateExtension<ChangeTriggersSqlServerDbContextOptionsExtension>();
            optionsBuilder.AsInfrastructure().AddOrUpdateExtension(extension);

            return optionsBuilder;
        }
    }
}