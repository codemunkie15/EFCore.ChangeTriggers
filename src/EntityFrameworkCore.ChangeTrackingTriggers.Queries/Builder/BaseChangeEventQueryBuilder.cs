using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Queries.Builder
{
    public abstract class BaseChangeEventQueryBuilder<TChangeEvent>
    {
        protected readonly DbContext context;
        protected List<IQueryable<TChangeEvent>> changeQueries = new();

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
    }
}
