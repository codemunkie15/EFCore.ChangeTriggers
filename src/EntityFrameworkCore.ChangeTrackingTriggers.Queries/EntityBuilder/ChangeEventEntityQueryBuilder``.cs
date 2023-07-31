using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using LinqKit;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Queries.EntityBuilder
{
    public class ChangeEventEntityQueryBuilder<TChange, TTracked, TChangeId, TChangedBy, TChangeSource>
        : BaseChangeEventEntityQueryBuilder<ChangeEvent<TChangedBy, TChangeSource>, TChange, TTracked, TChangeId>
        where TChange : class, IChange<TTracked, TChangeId>
    {
        public ChangeEventEntityQueryBuilder(DbContext context, IQueryable<TChange> dbSet)
            : base(context, dbSet)
        {
            if (!typeof(IHasChangedBy<TChangedBy>).IsAssignableFrom(typeof(TChange)) ||
                !typeof(IHasChangeSource<TChangeSource>).IsAssignableFrom(typeof(TChange)))
            {
                throw new ChangeTrackingTriggersQueryBuilderException(
                    $"The change entity type {typeof(TChange).Name} must implement {typeof(IHasChangedBy<>).Name} and {typeof(IHasChangeSource<>).Name} to use this builder. " +
                    $"Consider building a new query with the correct builder for this change entity.");
            }
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
                ChangedBy = ((IHasChangedBy<TChangedBy>)jc.ChangeEntity).ChangedBy,
                ChangeSource = ((IHasChangeSource<TChangeSource>)jc.ChangeEntity).ChangeSource
            });
        }
    }
}
