using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Infrastructure
{
    public class ChangeEventsDbContextOptionsBuilder
    {
        private readonly DbContextOptionsBuilder optionsBuilder;

        public ChangeEventsDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder)
        {
            this.optionsBuilder = optionsBuilder;
        }

        /// <summary>
        /// Configures the context to include insert events for every change entity.
        /// </summary>
        /// <param name="triggerNameFactory">Whether to include or exclude insert events for every change entity.</param>
        /// <returns>The options builder so that further calls can be chained.</returns>
        public ChangeEventsDbContextOptionsBuilder IncludeInserts(bool include = true)
            => WithOption(e => e.WithIncludeInserts(include));

        /// <summary>
        /// Configures the context to include delete events for every change entity.
        /// </summary>
        /// <param name="triggerNameFactory">Whether to include or exclude delete events for every change entity.</param>
        /// <returns>The options builder so that further calls can be chained.</returns>
        public ChangeEventsDbContextOptionsBuilder IncludeDeletes(bool include = true)
            => WithOption(e => e.WithIncludeDeletes(include));

        protected virtual ChangeEventsDbContextOptionsBuilder WithOption(Func<ChangeEventsDbContextOptionsExtension, ChangeEventsDbContextOptionsExtension> setAction)
        {
            var extension = setAction(optionsBuilder.GetOrCreateExtension<ChangeEventsDbContextOptionsExtension>());
            optionsBuilder.AsInfrastructure().AddOrUpdateExtension(extension);

            return this;
        }
    }
}