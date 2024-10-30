using EFCore.ChangeTriggers.ChangeEventQueries.Builders.PropertyBuilders;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders
{
    internal class ChangeEventQueryBuilder<TChangedBy, TChangeSource>
        : BaseChangeEventQueryBuilder<ChangeEvent<TChangedBy, TChangeSource>>
    {
        public ChangeEventQueryBuilder(
            ChangeEventQueryConfiguration configuration,
            IQueryable query)
            : base(configuration, query.ElementType, new ChangeEventQueryPropertyBuilder<TChangedBy, TChangeSource>(query))
        {
        }
    }
}