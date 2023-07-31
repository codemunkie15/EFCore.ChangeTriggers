using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
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

        public ChangeSourceChangeEventQueryBuilder<TChangeSource> AddEntityQuery<TChange, TTracked, TChangeId>(
            IQueryable<TChange> dbSet,
            Action<IChangeEventEntityQueryBuilder<TChange, ChangeSourceChangeEvent<TChangeSource>>> entityBuilder)
            where TChange : class, IChange<TTracked, TChangeId>, IHasChangeSource<TChangeSource>
        {
            var entityBuilderInstance = new ChangeSourceChangeEventEntityQueryBuilder<TChange, TTracked, TChangeId, TChangeSource>(context, dbSet);
            entityBuilder(entityBuilderInstance);

            changeQueries.Add(entityBuilderInstance.Build());

            return this;
        }
    }
}
