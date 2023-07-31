using EntityFrameworkCore.ChangeTrackingTriggers.Queries.EntityBuilder;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Queries.Builder
{
    public class ChangeEventQueryBuilder<TChangedBy, TChangeSource>
        : BaseChangeEventQueryBuilder<ChangeEvent<TChangedBy, TChangeSource>>
    {
        public ChangeEventQueryBuilder(DbContext context) : base(context)
        {
        }

        protected override IChangeEventEntityQueryBuilder<TChange, ChangeEvent<TChangedBy, TChangeSource>> CreateEntityBuilder<TChange, TTracked, TChangeId>(IQueryable<TChange> dbSet)
        {
            return new ChangeEventEntityQueryBuilder<TChange, TTracked, TChangeId, TChangedBy, TChangeSource>(context, dbSet);
        }
    }
}
