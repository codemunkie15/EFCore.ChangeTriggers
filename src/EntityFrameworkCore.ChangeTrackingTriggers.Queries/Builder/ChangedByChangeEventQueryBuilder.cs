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

        public ChangedByChangeEventQueryBuilder<TChangedBy> AddEntityQuery<TChange>(
            IQueryable<TChange> changes,
            Action<IChangeEventEntityQueryBuilder<TChange, ChangedByChangeEvent<TChangedBy>>> entityBuilder)
            where TChange : IChange, IHasChangedBy<TChangedBy>
        {
            var entityBuilderInstance = new ChangedByChangeEventEntityQueryBuilder<TChange, TChangedBy>(context, changes);
            entityBuilder(entityBuilderInstance);

            changeQueries.Add(entityBuilderInstance.Build());

            return this;
        }
    }
}
