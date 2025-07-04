using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.PropertyBuilders
{
    internal interface IPropertyQueryBuilder<TChangeEvent>
    {
        IQueryable<TChangeEvent> Build(ChangeEventEntityPropertyConfiguration propertyConfiguration);
    }
}
