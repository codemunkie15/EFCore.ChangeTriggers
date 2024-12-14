using EFCore.ChangeTriggers.ChangeEventQueries.Builders.PropertyBuilders;
using EFCore.ChangeTriggers.ChangeEventQueries.ChangedBy;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders
{
    internal class ChangedByChangeEventQueryBuilder<TChangedBy>
        : BaseChangeEventQueryBuilder<ChangeEvent<TChangedBy>>
    {
        public ChangedByChangeEventQueryBuilder(IQueryable query, ChangeEventConfiguration configuration)
            : base(query, new ChangedByChangeEventQueryPropertyBuilder<TChangedBy>(query), configuration)
        {
        }

        public ChangedByChangeEventQueryBuilder(IQueryable query)
            : base(query, new ChangedByChangeEventQueryPropertyBuilder<TChangedBy>(query))
        {
        }
    }
}