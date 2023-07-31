using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using EntityFrameworkCore.ChangeTrackingTriggers.Queries.EntityBuilder;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Queries.Builder
{
    public abstract class BaseChangeEventQueryBuilder<TChangeEvent>
        : IChangeEventQueryBuilder<TChangeEvent>
    {
        protected readonly DbContext context;
        private List<IQueryable<TChangeEvent>> changeQueries = new();

        public BaseChangeEventQueryBuilder(DbContext context)
        {
            this.context = context;
        }

        public IChangeEventQueryBuilder<TChangeEvent> AddEntityQuery<TChange, TTracked, TChangeId>(
            IQueryable<TChange> dbSet,
            Action<IChangeEventEntityQueryBuilder<TChange, TChangeEvent>> entityBuilder)
            where TChange : class, IChange<TTracked, TChangeId>
        {
            var entityBuilderInstance = CreateEntityBuilder<TChange, TTracked, TChangeId>(dbSet);
            entityBuilder(entityBuilderInstance);

            changeQueries.Add(entityBuilderInstance.Build());

            return this;
        }

        public IQueryable<TChangeEvent> Build()
        {
            if (!changeQueries.Any())
            {
                throw new InvalidOperationException("There are no queries configured to build.");
            }

            return changeQueries.Aggregate(Queryable.Concat);
        }

        protected abstract IChangeEventEntityQueryBuilder<TChange, TChangeEvent> CreateEntityBuilder<TChange, TTracked, TChangeId>(IQueryable<TChange> dbSet)
            where TChange : class, IChange<TTracked, TChangeId>;
    }
}
