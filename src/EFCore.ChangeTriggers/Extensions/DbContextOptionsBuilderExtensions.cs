using EFCore.ChangeTriggers.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace EFCore.ChangeTriggers
#pragma warning restore IDE0130 // Namespace does not match folder structure
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