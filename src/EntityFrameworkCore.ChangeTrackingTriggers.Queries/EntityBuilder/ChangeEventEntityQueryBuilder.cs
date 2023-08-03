using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using LinqKit;

namespace EntityFrameworkCore.ChangeTrackingTriggers.ChangeEventQueries.EntityBuilder
{
    public class ChangeEventEntityQueryBuilder<TChange>
        : BaseChangeEventEntityQueryBuilder<ChangeEvent, TChange>
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
