using EFCore.ChangeTriggers.ChangeEventQueries.Infrastructure;
using EFCore.ChangeTriggers.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Reflection;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace EFCore.ChangeTriggers.ChangeEventQueries
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    public static class ChangeTriggersDbContextOptionsBuilderExtensions
    {
        public static void UseChangeEventQueries<TBuilder, TExtension>(
            this ChangeTriggersDbContextOptionsBuilder<TBuilder, TExtension> builder,
            Assembly configurationsAssembly,
            Action<ChangeEventsDbContextOptionsBuilder>? optionsAction = null)
        where TBuilder : ChangeTriggersDbContextOptionsBuilder<TBuilder, TExtension>
        where TExtension : ChangeTriggersDbContextOptionsExtension, new()
        {
            var extension = builder.OptionsBuilder.GetOrCreateExtension<ChangeEventsDbContextOptionsExtension>()
                .WithConfigurationsAssembly(configurationsAssembly);

            builder.OptionsBuilder.AsInfrastructure().AddOrUpdateExtension(extension);

            builder.OptionsBuilder
                .ApplyConfiguration(optionsAction)
                .ReplaceService<IAsyncQueryProvider, DbContextAwareEntityQueryProvider>();
        }

        private static DbContextOptionsBuilder ApplyConfiguration(this DbContextOptionsBuilder optionsBuilder,
            Action<ChangeEventsDbContextOptionsBuilder>? optionsAction)
        {
            optionsAction?.Invoke(new ChangeEventsDbContextOptionsBuilder(optionsBuilder));

            var extension = optionsBuilder.GetOrCreateExtension<ChangeEventsDbContextOptionsExtension>();
            optionsBuilder.AsInfrastructure().AddOrUpdateExtension(extension);

            return optionsBuilder;
        }
    }
}