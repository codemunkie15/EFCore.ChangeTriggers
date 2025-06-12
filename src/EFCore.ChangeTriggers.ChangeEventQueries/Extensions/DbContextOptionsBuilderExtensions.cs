using EFCore.ChangeTriggers.ChangeEventQueries.Infrastructure;
using EFCore.ChangeTriggers.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Reflection;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace EFCore.ChangeTriggers.ChangeEventQueries
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Configures the DbContext to use change event queries.
        /// </summary>
        /// <param name="changeTriggersOptionsBuilder">The builder being used to configure the context.</param>
        /// <returns>The options builder so that further configuration can be chained.</returns>
        public static void UseChangeEventQueries<TBuilder, TExtension>(
            this ChangeTriggersDbContextOptionsBuilder<TBuilder, TExtension> changeTriggersOptionsBuilder,
            Action<ChangeEventsDbContextOptionsBuilder>? optionsAction = null)
        where TBuilder : ChangeTriggersDbContextOptionsBuilder<TBuilder, TExtension>
        where TExtension : ChangeTriggersDbContextOptionsExtension, new()
        {
            var extension = changeTriggersOptionsBuilder.OptionsBuilder.GetOrCreateExtension<ChangeEventsDbContextOptionsExtension>();

            changeTriggersOptionsBuilder.UseChangeEventQueries(extension);
        }

        /// <summary>
        /// Configures the DbContext to use change event queries.
        /// </summary>
        /// <param name="changeTriggersOptionsBuilder">The builder being used to configure the context.</param>
        /// <param name="configurationsAssembly">The assembly where your change event configurations are located.</param>
        /// <returns>The options builder so that further configuration can be chained.</returns>
        public static void UseChangeEventQueries<TBuilder, TExtension>(
            this ChangeTriggersDbContextOptionsBuilder<TBuilder, TExtension> changeTriggersOptionsBuilder,
            Assembly configurationsAssembly,
            Action<ChangeEventsDbContextOptionsBuilder>? optionsAction = null)
        where TBuilder : ChangeTriggersDbContextOptionsBuilder<TBuilder, TExtension>
        where TExtension : ChangeTriggersDbContextOptionsExtension, new()
        {
            var extension = changeTriggersOptionsBuilder.OptionsBuilder.GetOrCreateExtension<ChangeEventsDbContextOptionsExtension>()
                .WithConfigurationsAssembly(configurationsAssembly);

            changeTriggersOptionsBuilder.UseChangeEventQueries(extension);
        }

        private static void UseChangeEventQueries<TBuilder, TExtension>(
            this ChangeTriggersDbContextOptionsBuilder<TBuilder, TExtension> changeTriggersOptionsBuilder,
            ChangeEventsDbContextOptionsExtension extension,
            Action<ChangeEventsDbContextOptionsBuilder>? optionsAction = null)
        where TBuilder : ChangeTriggersDbContextOptionsBuilder<TBuilder, TExtension>
        where TExtension : ChangeTriggersDbContextOptionsExtension, new()
        {
            var optionsBuilder = changeTriggersOptionsBuilder.OptionsBuilder;
            optionsBuilder.AsInfrastructure().AddOrUpdateExtension(extension);

            optionsBuilder
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