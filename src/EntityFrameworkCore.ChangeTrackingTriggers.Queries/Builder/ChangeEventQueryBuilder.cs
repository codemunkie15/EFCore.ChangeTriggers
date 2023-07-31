using EntityFrameworkCore.ChangeTrackingTriggers.Queries.EntityBuilder;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Queries.Builder
{
    public class ChangeEventQueryBuilder
        : BaseChangeEventQueryBuilder<ChangeEvent>
    {
        public ChangeEventQueryBuilder(DbContext context) : base(context)
        {
        }

        protected override IChangeEventEntityQueryBuilder<TChange, ChangeEvent> CreateEntityBuilder<TChange, TTracked, TChangeId>(IQueryable<TChange> dbSet)
        {
            return new ChangeEventEntityQueryBuilder<TChange, TTracked, TChangeId>(context, dbSet);
        }
    }
}
