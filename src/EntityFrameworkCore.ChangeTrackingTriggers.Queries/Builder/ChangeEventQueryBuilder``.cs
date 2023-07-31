using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
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

        public ChangeEventQueryBuilder<TChangedBy, TChangeSource> AddEntityQuery<TChange, TTracked, TChangeId>(
            IQueryable<TChange> dbSet,
            Action<IChangeEventEntityQueryBuilder<TChange, ChangeEvent<TChangedBy, TChangeSource>>> entityBuilder)
            where TChange : class, IChange<TTracked, TChangeId>, IHasChangedBy<TChangedBy>, IHasChangeSource<TChangeSource>
        {
            var entityBuilderInstance = new ChangeEventEntityQueryBuilder<TChange, TTracked, TChangeId, TChangedBy, TChangeSource>(context, dbSet);
            entityBuilder(entityBuilderInstance);

            changeQueries.Add(entityBuilderInstance.Build());

            return this;
        }
    }
}
