using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using EntityFrameworkCore.ChangeTrackingTriggers.Extensions;
using EntityFrameworkCore.ChangeTrackingTriggers.ChangeEventQueries.EntityBuilder;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EntityFrameworkCore.ChangeTrackingTriggers.ChangeEventQueries.Builder
{
    public class ChangeEventQueryBuilder<TChangedBy, TChangeSource>
        : BaseChangeEventQueryBuilder<ChangeEvent<TChangedBy, TChangeSource>>
    {
        public ChangeEventQueryBuilder(DbContext context) : base(context)
        {
        }

        public ChangeEventQueryBuilder<TChangedBy, TChangeSource> AddEntityQuery<TChange>(
            IQueryable<TChange> changes,
            Action<IChangeEventEntityQueryBuilder<TChange, ChangeEvent<TChangedBy, TChangeSource>>> entityQueryBuilder)
            where TChange : IChange, IHasChangedBy<TChangedBy>, IHasChangeSource<TChangeSource>
        {
            var entityBuilderInstance = new ChangeEventEntityQueryBuilder<TChange, TChangedBy, TChangeSource>(context, changes);
            entityQueryBuilder(entityBuilderInstance);

            AddChanges(entityBuilderInstance);

            return this;
        }
    }
}
