using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Diagnostics;

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
            where TChangedByProvider : class, IChangedByProvider<TChangedBy>
        {
            var applicationServiceProvider = optionsBuilder.Options.FindExtension<CoreOptionsExtension>()?.ApplicationServiceProvider;
            return WithOption(e => (TExtension)e.WithChangedBy<TChangedByProvider, TChangedBy>(applicationServiceProvider));
        }

        public TBuilder UseChangeSource<TChangeSourceProvider, TChangeSource>()
            where TChangeSourceProvider : class, IChangeSourceProvider<TChangeSource>
        {
            var applicationServiceProvider = optionsBuilder.Options.FindExtension<CoreOptionsExtension>()?.ApplicationServiceProvider;
            return WithOption(e => (TExtension)e.WithChangeSource<TChangeSourceProvider, TChangeSource>(applicationServiceProvider));
        }

        protected virtual TBuilder WithOption(Func<TExtension, TExtension> setAction)
        {
            var extension = setAction(optionsBuilder.GetOrCreateExtension<TExtension>());
            optionsBuilder.AsInfrastructure().AddOrUpdateExtension(extension);

            return (TBuilder)this;
        }
    }
}
