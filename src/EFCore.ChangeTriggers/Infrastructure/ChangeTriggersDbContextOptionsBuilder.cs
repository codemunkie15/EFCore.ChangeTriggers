using EFCore.ChangeTriggers.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.Infrastructure
{
    public abstract class ChangeTriggersDbContextOptionsBuilder<TBuilder, TExtension>
        where TBuilder : ChangeTriggersDbContextOptionsBuilder<TBuilder, TExtension>
        where TExtension : ChangeTriggersDbContextOptionsExtension, new()
    {
        internal DbContextOptionsBuilder OptionsBuilder { get; }

        public ChangeTriggersDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder)
        {
            this.OptionsBuilder = optionsBuilder;
        }

        /// <summary>
        /// Configures the context to use the provided function to generate change trigger names.
        /// </summary>
        /// <param name="triggerNameFactory">The trigger name factory.</param>
        /// <returns>The options builder so that further calls can be chained.</returns>
        public TBuilder UseTriggerNameFactory(Func<string, string> triggerNameFactory)
            => WithOption(e => (TExtension)e.WithTriggerNameFactory(triggerNameFactory));

        /// <summary>
        /// Configures the context to track who made changes to entities.
        /// </summary>
        /// <typeparam name="TChangedByProvider">The type of <see cref="IChangedByProvider{TChangedBy}"/> to use.</typeparam>
        /// <typeparam name="TChangedBy">The type that <typeparamref name="TChangedByProvider"/> returns.</typeparam>
        /// <returns>The options builder so that further calls can be chained.</returns>
        public TBuilder UseChangedBy<TChangedByProvider, TChangedBy>()
            where TChangedByProvider : class, IChangedByProvider<TChangedBy>
            => WithOption(e => (TExtension)e.WithChangedBy<TChangedByProvider, TChangedBy>());

        /// <summary>
        /// Configures the context to track where changes originated from.
        /// </summary>
        /// <typeparam name="TChangeSourceProvider">The type of <see cref="IChangeSourceProvider{TChangeSource}"/> to use.</typeparam>
        /// <typeparam name="TChangeSource">The type that <typeparamref name="TChangeSourceProvider"/> returns.</typeparam>
        /// <returns>The options builder so that further calls can be chained.</returns>
        public TBuilder UseChangeSource<TChangeSourceProvider, TChangeSource>()
            where TChangeSourceProvider : class, IChangeSourceProvider<TChangeSource>
            => WithOption(e => (TExtension)e.WithChangeSource<TChangeSourceProvider, TChangeSource>());

        protected virtual TBuilder WithOption(Func<TExtension, TExtension> setAction)
        {
            var extension = setAction(OptionsBuilder.GetOrCreateExtension<TExtension>());
            OptionsBuilder.AsInfrastructure().AddOrUpdateExtension(extension);

            return (TBuilder)this;
        }
    }
}
