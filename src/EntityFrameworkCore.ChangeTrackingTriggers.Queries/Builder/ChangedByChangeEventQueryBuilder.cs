using EntityFrameworkCore.ChangeTrackingTriggers.Queries.EntityBuilder;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Queries.Builder
{
    public class ChangedByChangeEventQueryBuilder<TChangedBy>
        : BaseChangeEventQueryBuilder<ChangedByChangeEvent<TChangedBy>>
    {
        public ChangedByChangeEventQueryBuilder(DbContext context) : base(context)
        {
        }

        protected override IChangeEventEntityQueryBuilder<TChange, ChangedByChangeEvent<TChangedBy>> CreateEntityBuilder<TChange, TTracked, TChangeId>(IQueryable<TChange> dbSet)
        {
            return new ChangedByChangeEventEntityQueryBuilder<TChange, TTracked, TChangeId, TChangedBy>(context, dbSet);
        }
    }
}
