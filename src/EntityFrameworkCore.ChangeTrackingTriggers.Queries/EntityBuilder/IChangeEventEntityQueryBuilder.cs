using System.Linq.Expressions;

namespace EntityFrameworkCore.ChangeTrackingTriggers.ChangeEventQueries.EntityBuilder
{
    public interface IChangeEventEntityQueryBuilder<TChange, TChangeEvent>
    {
        IChangeEventEntityQueryBuilder<TChange, TChangeEvent> AddProperty(
            string description,
            Expression<Func<TChange, string>> valueSelector);

        IChangeEventEntityQueryBuilder<TChange, TChangeEvent> AddEntityProperties(
            Func<string, string>? descriptionBuilder = null);

        IQueryable<TChangeEvent> Build();
    }
}
