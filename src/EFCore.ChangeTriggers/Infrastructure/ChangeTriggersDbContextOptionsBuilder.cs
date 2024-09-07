using EFCore.ChangeTriggers.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.Infrastructure
{
    public abstract class ChangeTriggersDbContextOptionsBuilder<TBuilder, TExtension>
        where TBuilder : ChangeTriggersDbContextOptionsBuilder<TBuilder, TExtension>
        where TExtension : ChangeTriggersDbContextOptionsExtension, new()
    {
        private DbContextOptionsBuilder optionsBuilder;

        public ChangeTriggersDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder)
        {
            this.optionsBuilder = optionsBuilder;
        }

        public TBuilder UseTriggerNameFactory(Func<string, string> triggerNameFactory)
            => WithOption(e => (TExtension)e.WithTriggerNameFactory(triggerNameFactory));

        public TBuilder UseChangedBy<TChangedByProvider, TChangedBy>()
            => WithOption(e => (TExtension)e.WithChangedBy<TChangedByProvider, TChangedBy>());

        public TBuilder UseChangeSource<TChangeSourceProvider, TChangeSource>()
            => WithOption(e => (TExtension)e.WithChangeSource<TChangeSourceProvider, TChangeSource>());

        protected virtual TBuilder WithOption(Func<TExtension, TExtension> setAction)
        {
            var extension = setAction(optionsBuilder.GetOrCreateExtension<TExtension>());
            optionsBuilder.AsInfrastructure().AddOrUpdateExtension(extension);

            return (TBuilder)this;
        }
    }
}
