using EFCore.ChangeTriggers.ChangeEventQueries.Builders.OperationTypeBuilders;
using EFCore.ChangeTriggers.ChangeEventQueries.Builders.PropertyBuilders;
using EFCore.ChangeTriggers.ChangeEventQueries.ChangedByEvents;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.RootQueryBuilders
{
    internal class ChangedByRootQueryBuilder<TChangedBy>
        : BaseRootQueryBuilder<ChangeEvent<TChangedBy>>
    {
        public ChangedByRootQueryBuilder(IQueryable query, ChangeEventConfiguration configuration)
            : base(query,
                   new ChangedByPropertyQueryBuilder<TChangedBy>(query),
                   new ChangedByOperationTypeQueryBuilder<TChangedBy>(query),
                   configuration)
        {
        }

        public ChangedByRootQueryBuilder(IQueryable query)
            : base(query,
                  new ChangedByPropertyQueryBuilder<TChangedBy>(query),
                  new ChangedByOperationTypeQueryBuilder<TChangedBy>(query))
        {
        }
    }
}