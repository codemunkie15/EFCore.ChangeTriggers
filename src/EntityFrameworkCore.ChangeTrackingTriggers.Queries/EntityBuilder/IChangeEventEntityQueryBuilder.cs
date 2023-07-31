using System.Linq.Expressions;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Queries.EntityBuilder
{
    public interface IChangeEventEntityQueryBuilder<TChange, TChangeEvent>
    {
        IChangeEventEntityQueryBuilder<TChange, TChangeEvent> AddProperty(
            string description,
            Expression<Func<TChange, string>> valueSelector);

        IQueryable<TChangeEvent> Build();
    }
}
