using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using EntityFrameworkCore.ChangeTrackingTriggers.ChangeEventQueries.EntityBuilder;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ChangeTrackingTriggers.ChangeEventQueries.Builder
{
    public class ChangeEventQueryBuilder
        : BaseChangeEventQueryBuilder<ChangeEvent>
    {
        public ChangeEventQueryBuilder(DbContext context) : base(context)
        {
        }

        public ChangeEventQueryBuilder AddEntityQuery<TChange>(
            IQueryable<TChange> changes,
            Action<IChangeEventEntityQueryBuilder<TChange, ChangeEvent>> entityQueryBuilder)
            where TChange : IChange
        {
            var entityBuilderInstance = new ChangeEventEntityQueryBuilder<TChange>(context, changes);
            entityQueryBuilder(entityBuilderInstance);

            AddChanges(entityBuilderInstance);

            return this;
        }
    }
}
