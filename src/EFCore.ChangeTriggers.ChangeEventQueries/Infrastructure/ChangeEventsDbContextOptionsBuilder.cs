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

        public ChangeEventsDbContextOptionsBuilder IncludeInserts(bool include = true)
            => WithOption(e => e.WithIncludeInserts(include));

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