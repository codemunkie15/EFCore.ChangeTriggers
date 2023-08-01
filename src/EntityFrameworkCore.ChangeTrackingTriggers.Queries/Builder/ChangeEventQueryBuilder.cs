using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
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

        public ChangeEventQueryBuilder AddEntityQuery<TChange>(
            IQueryable<TChange> changes,
            Action<IChangeEventEntityQueryBuilder<TChange, ChangeEvent>> entityBuilder)
            where TChange : IChange
        {
            var entityBuilderInstance = new ChangeEventEntityQueryBuilder<TChange>(context, changes);
            entityBuilder(entityBuilderInstance);

            changeQueries.Add(entityBuilderInstance.Build());

            return this;
        }
    }
}
