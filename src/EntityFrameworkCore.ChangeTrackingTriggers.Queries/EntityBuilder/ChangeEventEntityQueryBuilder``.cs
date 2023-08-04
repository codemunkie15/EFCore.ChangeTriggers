using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using LinqKit;

namespace EntityFrameworkCore.ChangeTrackingTriggers.ChangeEventQueries.EntityBuilder
{
    /// <inheritdoc/>
    /// <typeparam name="TChangedBy">The type that represents who a change was made by.</typeparam>
    /// <typeparam name="TChangeSource">The type that represents where a change originated from.</typeparam>
    public class ChangeEventEntityQueryBuilder<TChange, TChangedBy, TChangeSource>
        : BaseChangeEventEntityQueryBuilder<TChange, ChangeEvent<TChangedBy, TChangeSource>>
        where TChange : IChange, IHasChangedBy<TChangedBy>, IHasChangeSource<TChangeSource>
    {
        public ChangeEventEntityQueryBuilder(DbContext context, IQueryable<TChange> dbSet)
            : base(context, dbSet)
        {
        }

        /// <inheritdoc/>
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
