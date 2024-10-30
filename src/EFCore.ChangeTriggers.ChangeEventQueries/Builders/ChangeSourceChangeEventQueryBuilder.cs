using EFCore.ChangeTriggers.ChangeEventQueries.Builders.PropertyBuilders;
using EFCore.ChangeTriggers.ChangeEventQueries.ChangeSource;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders
{
    internal class ChangeSourceChangeEventQueryBuilder<TChangeSource>
        : BaseChangeEventQueryBuilder<ChangeEvent<TChangeSource>>
    {
        public ChangeSourceChangeEventQueryBuilder(
            ChangeEventQueryConfiguration configuration,
            IQueryable query)
            : base(configuration, query.ElementType, new ChangeSourceChangeEventQueryPropertyBuilder<TChangeSource>(query))
        {
        }
    }
}