using EFCore.ChangeTriggers.Migrations;
using EFCore.ChangeTriggers.Migrations.Migrators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore.ChangeTriggers.Extensions
{
    internal static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseChangeTriggers(this DbContextOptionsBuilder optionsBuilder)
        {
            return optionsBuilder
                .ReplaceService<IMigrationsModelDiffer, ChangeTriggersMigrationsModelDiffer>()
                .ReplaceService<IMigrator, ChangeTriggersMigrator>()
                .ReplaceService<IModelCustomizer, ChangeTriggersModelCustomizer>();
        }

        public static IDbContextOptionsBuilderInfrastructure AsInfrastructure(this DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder;

        public static TExtension GetOrCreateExtension<TExtension>(this DbContextOptionsBuilder optionsBuilder)
            where TExtension : class, IDbContextOptionsExtension, new()
        {
            return optionsBuilder.Options.FindExtension<TExtension>() ?? new TExtension();
        }
    }
}