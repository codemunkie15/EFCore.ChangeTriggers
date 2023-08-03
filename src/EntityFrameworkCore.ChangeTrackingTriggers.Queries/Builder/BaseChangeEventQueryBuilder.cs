using EntityFrameworkCore.ChangeTrackingTriggers.ChangeEventQueries.EntityBuilder;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ChangeTrackingTriggers.ChangeEventQueries.Builder
{
    public abstract class BaseChangeEventQueryBuilder<TChangeEvent>
    {
        protected readonly DbContext context;
        private List<IQueryable<TChangeEvent>> changeQueries = new();

        public BaseChangeEventQueryBuilder(DbContext context)
        {
            this.context = context;
        }

        public IQueryable<TChangeEvent> Build()
        {
            if (!changeQueries.Any())
            {
                throw new InvalidOperationException("There are no queries configured to build.");
            }

            return changeQueries.Aggregate(Queryable.Concat);
        }

        protected void AddChanges<TChange>(IChangeEventEntityQueryBuilder<TChange, TChangeEvent> entityQueryBuilder)
        {
            changeQueries.Add(entityQueryBuilder.Build());
        }
    }
}
