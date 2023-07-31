using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
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

        public ChangedByChangeEventQueryBuilder<TChangedBy> AddEntityQuery<TChange, TTracked, TChangeId>(
            IQueryable<TChange> dbSet,
            Action<IChangeEventEntityQueryBuilder<TChange, ChangedByChangeEvent<TChangedBy>>> entityBuilder)
            where TChange : class, IChange<TTracked, TChangeId>, IHasChangedBy<TChangedBy>
        {
            var entityBuilderInstance = new ChangedByChangeEventEntityQueryBuilder<TChange, TTracked, TChangeId, TChangedBy>(context, dbSet);
            entityBuilder(entityBuilderInstance);

            changeQueries.Add(entityBuilderInstance.Build());

            return this;
        }
    }
}
