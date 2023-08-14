using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using LinqKit;
using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.ChangeEventQueries.EntityBuilder
{
    /// <inheritdoc/>
    public class ChangeEventEntityQueryBuilder<TChange>
        : BaseChangeEventEntityQueryBuilder<TChange, ChangeEvent>
        where TChange : IChange
    {
        public ChangeEventEntityQueryBuilder(DbContext context, IQueryable<TChange> dbSet)
            : base(context, dbSet)
        {
        }

        protected override IQueryable<ChangeEvent> ProjectToResult(
            IQueryable<JoinedChanges<TChange>> query,
            string description,
            Expression<Func<TChange, string>> valueSelector)
        {
            return query.Select(jc => new ChangeEvent
            {
                Description = description,
                OldValue = valueSelector.Invoke(jc.PreviousChangeEntity),
                NewValue = valueSelector.Invoke(jc.ChangeEntity),
                ChangedAt = jc.ChangeEntity.ChangedAt,
            });
        }
    }
}
