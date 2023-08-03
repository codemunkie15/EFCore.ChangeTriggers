using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using EntityFrameworkCore.ChangeTrackingTriggers.ChangeEventQueries.EntityBuilder;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ChangeTrackingTriggers.ChangeEventQueries.Builder
{
    public class ChangeSourceChangeEventQueryBuilder<TChangeSource>
        : BaseChangeEventQueryBuilder<ChangeSourceChangeEvent<TChangeSource>>
    {
        public ChangeSourceChangeEventQueryBuilder(DbContext context) : base(context)
        {
        }

        public ChangeSourceChangeEventQueryBuilder<TChangeSource> AddEntityQuery<TChange>(
            IQueryable<TChange> changes,
            Action<IChangeEventEntityQueryBuilder<TChange, ChangeSourceChangeEvent<TChangeSource>>> entityQueryBuilder)
            where TChange : IChange, IHasChangeSource<TChangeSource>
        {
            var entityBuilderInstance = new ChangeSourceChangeEventEntityQueryBuilder<TChange, TChangeSource>(context, changes);
            entityQueryBuilder(entityBuilderInstance);

            AddChanges(entityBuilderInstance);

            return this;
        }
    }
}
