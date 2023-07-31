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

        public ChangeEventQueryBuilder AddEntityQuery<TChange, TTracked, TChangeId>(
            IQueryable<TChange> dbSet,
            Action<IChangeEventEntityQueryBuilder<TChange, ChangeEvent>> entityBuilder)
            where TChange : class, IChange<TTracked, TChangeId>
        {
            var entityBuilderInstance = new ChangeEventEntityQueryBuilder<TChange, TTracked, TChangeId>(context, dbSet);
            entityBuilder(entityBuilderInstance);

            changeQueries.Add(entityBuilderInstance.Build());

            return this;
        }
    }
}
