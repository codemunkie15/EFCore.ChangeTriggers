using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using LinqKit;

namespace EntityFrameworkCore.ChangeTrackingTriggers.ChangeEventQueries.EntityBuilder
{
    /// <inheritdoc/>
    /// <typeparam name="TChangeSource">The type that represents where a change originated from.</typeparam>
    public class ChangeSourceChangeEventEntityQueryBuilder<TChange, TChangeSource>
        : BaseChangeEventEntityQueryBuilder<TChange, ChangeSourceChangeEvent<TChangeSource>>
        where TChange : IChange, IHasChangeSource<TChangeSource>
    {
        public ChangeSourceChangeEventEntityQueryBuilder(DbContext context, IQueryable<TChange> dbSet)
            : base(context, dbSet)
        {
        }

        /// <inheritdoc/>
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
