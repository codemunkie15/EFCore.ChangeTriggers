using EFCore.ChangeTriggers.ChangeEventQueries.Builders.PropertyBuilders;
using EFCore.ChangeTriggers.ChangeEventQueries.ChangedBy;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders
{
    internal class ChangedByChangeEventQueryBuilder<TChangedBy>
        : BaseChangeEventQueryBuilder<ChangeEvent<TChangedBy>>
    {
        public ChangedByChangeEventQueryBuilder(
            ChangeEventQueryConfiguration configuration,
            IQueryable query)
            : base(configuration, query.ElementType, new ChangedByChangeEventQueryPropertyBuilder<TChangedBy>(query))
        {
        }
    }
}