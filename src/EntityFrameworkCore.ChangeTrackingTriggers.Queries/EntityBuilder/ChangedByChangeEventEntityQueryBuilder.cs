using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using LinqKit;

namespace EntityFrameworkCore.ChangeTrackingTriggers.ChangeEventQueries.EntityBuilder
{
    /// <inheritdoc/>
    /// <typeparam name="TChange">The change entity type.</typeparam>
    /// <typeparam name="TChangedBy">The type that represents who a change was made by.</typeparam>
    public class ChangedByChangeEventEntityQueryBuilder<TChange, TChangedBy>
        : BaseChangeEventEntityQueryBuilder<TChange, ChangedByChangeEvent<TChangedBy>>
        where TChange : IChange, IHasChangedBy<TChangedBy>
    {
        public ChangedByChangeEventEntityQueryBuilder(DbContext context, IQueryable<TChange> dbSet)
            : base(context, dbSet)
        {
        }

        /// <inheritdoc/>
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
                ChangedBy = jc.ChangeEntity.ChangedBy
            });
        }
    }
}
