using EntityFrameworkCore.ChangeTrackingTriggers.Queries.EntityBuilder;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Queries.Builder
{
    public class ChangeSourceChangeEventQueryBuilder<TChangeSource>
        : BaseChangeEventQueryBuilder<ChangeSourceChangeEvent<TChangeSource>>
    {
        public ChangeSourceChangeEventQueryBuilder(DbContext context) : base(context)
        {
        }

        protected override IChangeEventEntityQueryBuilder<TChange, ChangeSourceChangeEvent<TChangeSource>> CreateEntityBuilder<TChange, TTracked, TChangeId>(IQueryable<TChange> dbSet)
        {
            return new ChangeSourceChangeEventEntityQueryBuilder<TChange, TTracked, TChangeId, TChangeSource>(context, dbSet);
        }
    }
}
