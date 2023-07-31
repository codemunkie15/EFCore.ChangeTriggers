using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using LinqKit;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Queries.EntityBuilder
{
    public class ChangeEventEntityQueryBuilder<TChange, TTracked, TChangeId, TChangedBy, TChangeSource>
        : BaseChangeEventEntityQueryBuilder<ChangeEvent<TChangedBy, TChangeSource>, TChange, TTracked, TChangeId>
        where TChange : class, IChange<TTracked, TChangeId>, IHasChangedBy<TChangedBy>, IHasChangeSource<TChangeSource>
    {
        public ChangeEventEntityQueryBuilder(DbContext context, IQueryable<TChange> dbSet)
            : base(context, dbSet)
        {
        }

        protected override IQueryable<ChangeEvent<TChangedBy, TChangeSource>> ProjectToResult(
            IQueryable<JoinedChanges<TChange>> query,
            string description,
            Expression<Func<TChange, string>> valueSelector)
        {
            return query.Select(jc => new ChangeEvent<TChangedBy, TChangeSource>
            {
                Description = description,
                OldValue = valueSelector.Invoke(jc.PreviousChangeEntity),
                NewValue = valueSelector.Invoke(jc.ChangeEntity),
                ChangedAt = jc.ChangeEntity.ChangedAt,
                ChangedBy = jc.ChangeEntity.ChangedBy,
                ChangeSource = jc.ChangeEntity.ChangeSource
            });
        }
    }
}
