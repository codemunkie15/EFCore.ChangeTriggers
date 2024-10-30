using System.Linq.Expressions;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.PropertyBuilders
{
    public interface IChangeEventQueryPropertyBuilder<TChangeEvent>
    {
        IQueryable<TChangeEvent> BuildChangeEventQuery(LambdaExpression valueSelector);
    }
}
