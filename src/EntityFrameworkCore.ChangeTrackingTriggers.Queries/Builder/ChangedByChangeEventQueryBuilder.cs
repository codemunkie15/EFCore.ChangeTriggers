using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using EntityFrameworkCore.ChangeTrackingTriggers.ChangeEventQueries.EntityBuilder;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ChangeTrackingTriggers.ChangeEventQueries.Builder
{
    public class ChangedByChangeEventQueryBuilder<TChangedBy>
        : BaseChangeEventQueryBuilder<ChangedByChangeEvent<TChangedBy>>
    {
        public ChangedByChangeEventQueryBuilder(DbContext context) : base(context)
        {
        }

        public ChangedByChangeEventQueryBuilder<TChangedBy> AddChanges<TChange>(
            IQueryable<TChange> changes,
            Action<IChangeEventEntityQueryBuilder<TChange, ChangedByChangeEvent<TChangedBy>>> entityQueryBuilder)
            where TChange : IChange, IHasChangedBy<TChangedBy>
        {
            var entityBuilderInstance = new ChangedByChangeEventEntityQueryBuilder<TChange, TChangedBy>(context, changes);
            entityQueryBuilder(entityBuilderInstance);

            AddChanges(entityBuilderInstance);

            return this;
        }
    }
}
