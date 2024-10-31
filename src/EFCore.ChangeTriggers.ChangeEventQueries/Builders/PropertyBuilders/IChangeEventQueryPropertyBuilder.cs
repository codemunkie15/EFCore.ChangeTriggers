using System.Linq.Expressions;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.PropertyBuilders
{
    internal interface IChangeEventQueryPropertyBuilder<TChangeEvent>
    {
        IQueryable<TChangeEvent> BuildChangeEventQuery(LambdaExpression valueSelector);
    }
}
