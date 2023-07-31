using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using LinqKit;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Queries.EntityBuilder
{
    public class ChangeSourceChangeEventEntityQueryBuilder<TChange, TTracked, TChangeId, TChangeSource>
        : BaseChangeEventEntityQueryBuilder<ChangeSourceChangeEvent<TChangeSource>, TChange, TTracked, TChangeId>
        where TChange : class, IChange<TTracked, TChangeId>, IHasChangeSource<TChangeSource>
    {
        public ChangeSourceChangeEventEntityQueryBuilder(DbContext context, IQueryable<TChange> dbSet)
            : base(context, dbSet)
        {
        }

        protected override IQueryable<ChangeSourceChangeEvent<TChangeSource>> ProjectToResult(
            IQueryable<JoinedChanges<TChange>> query,
            string description,
            Expression<Func<TChange, string>> valueSelector)
        {
            return query.Select(jc => new ChangeSourceChangeEvent<TChangeSource>
            {
                Description = description,
                OldValue = valueSelector.Invoke(jc.PreviousChangeEntity),
                NewValue = valueSelector.Invoke(jc.ChangeEntity),
                ChangedAt = jc.ChangeEntity.ChangedAt,
                ChangeSource = jc.ChangeEntity.ChangeSource
            });
        }
    }
}
