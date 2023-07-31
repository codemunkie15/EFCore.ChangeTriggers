using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using EntityFrameworkCore.ChangeTrackingTriggers.Queries.EntityBuilder;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Queries.Builder
{
    public interface IChangeEventQueryBuilder<TChangeEvent>
    {
        IChangeEventQueryBuilder<TChangeEvent> AddEntityQuery<TChange, TTracked, TChangeId>(
            IQueryable<TChange> dbSet,
            Action<IChangeEventEntityQueryBuilder<TChange, TChangeEvent>> entityBuilder)
            where TChange : class, IChange<TTracked, TChangeId>;

        IQueryable<TChangeEvent> Build();
    }
}
