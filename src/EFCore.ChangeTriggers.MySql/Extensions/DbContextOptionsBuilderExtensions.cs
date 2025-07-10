using EFCore.ChangeTriggers.MySql.Infrastructure;
using EFCore.ChangeTriggers.MySql.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace EFCore.ChangeTriggers.MySql
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Configures the DbContext to use change triggers.
        /// </summary>
        /// <param name="optionsBuilder">The builder being used to configure the context.</param>
        /// <returns>The options builder so that further configuration can be chained.</returns>
        public static DbContextOptionsBuilder UseMySqlChangeTriggers(
            this DbContextOptionsBuilder optionsBuilder,
            Action<ChangeTriggersMySqlDbContextOptionsBuilder>? changeTriggersOptionsAction = null)
        {
            var extension = optionsBuilder.GetOrCreateExtension<ChangeTriggersMySqlDbContextOptionsExtension>();
            optionsBuilder.AsInfrastructure().AddOrUpdateExtension(extension);

            return optionsBuilder
                .UseChangeTriggers()
                .ApplyConfiguration(changeTriggersOptionsAction)
                .ReplaceService<IMigrationsSqlGenerator, ChangeTriggersMySqlMigrationsSqlGenerator>();
        }

        private static DbContextOptionsBuilder ApplyConfiguration(this DbContextOptionsBuilder optionsBuilder,
            Action<ChangeTriggersMySqlDbContextOptionsBuilder>? changeTriggersOptionsAction)
        {
            changeTriggersOptionsAction?.Invoke(new ChangeTriggersMySqlDbContextOptionsBuilder(optionsBuilder));

            var extension = optionsBuilder.GetOrCreateExtension<ChangeTriggersMySqlDbContextOptionsExtension>();
            optionsBuilder.AsInfrastructure().AddOrUpdateExtension(extension);

            return optionsBuilder;
        }
    }
}