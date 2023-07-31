using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using LinqKit;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Queries.EntityBuilder
{
    public class ChangedByChangeEventEntityQueryBuilder<TChange, TTracked, TChangeId, TChangedBy>
        : BaseChangeEventEntityQueryBuilder<ChangedByChangeEvent<TChangedBy>, TChange, TTracked, TChangeId>
        where TChange : class, IChange<TTracked, TChangeId>, IHasChangedBy<TChangedBy>
    {
        public ChangedByChangeEventEntityQueryBuilder(DbContext context, IQueryable<TChange> dbSet)
            : base(context, dbSet)
        {
        }

        protected override IQueryable<ChangedByChangeEvent<TChangedBy>> ProjectToResult(
            IQueryable<JoinedChanges<TChange>> query,
            string description,
            Expression<Func<TChange, string>> valueSelector)
        {
            return query.Select(jc => new ChangedByChangeEvent<TChangedBy>
            {
                Description = description,
                OldValue = valueSelector.Invoke(jc.PreviousChangeEntity),
                NewValue = valueSelector.Invoke(jc.ChangeEntity),
                ChangedAt = jc.ChangeEntity.ChangedAt,
                ChangedBy = ((IHasChangedBy<TChangedBy>)jc.ChangeEntity).ChangedBy
            });
        }
    }
}
