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

        public ChangeSourceChangeEventQueryBuilder<TChangeSource> AddEntityQuery<TChange>(
            IQueryable<TChange> changes,
            Action<IChangeEventEntityQueryBuilder<TChange, ChangeSourceChangeEvent<TChangeSource>>> entityBuilder)
            where TChange : IChange, IHasChangeSource<TChangeSource>
        {
            var entityBuilderInstance = new ChangeSourceChangeEventEntityQueryBuilder<TChange, TChangeSource>(context, changes);
            entityBuilder(entityBuilderInstance);

            changeQueries.Add(entityBuilderInstance.Build());

            return this;
        }
    }
}
