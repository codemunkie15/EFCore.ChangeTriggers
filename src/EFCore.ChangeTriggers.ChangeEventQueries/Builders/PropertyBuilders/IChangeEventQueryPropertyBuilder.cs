using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.PropertyBuilders
{
    internal interface IChangeEventQueryPropertyBuilder<TChangeEvent>
    {
        IQueryable<TChangeEvent> Build(ChangeEventEntityPropertyConfiguration propertyConfiguration);
    }
}
