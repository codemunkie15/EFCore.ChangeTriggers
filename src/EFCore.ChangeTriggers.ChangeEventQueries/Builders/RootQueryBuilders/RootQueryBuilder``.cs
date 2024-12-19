using EFCore.ChangeTriggers.ChangeEventQueries.Builders.OperationTypeBuilders;
using EFCore.ChangeTriggers.ChangeEventQueries.Builders.PropertyBuilders;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.RootQueryBuilders
{
    internal class RootQueryBuilder<TChangedBy, TChangeSource>
        : BaseRootQueryBuilder<ChangeEvent<TChangedBy, TChangeSource>>
    {
        public RootQueryBuilder(IQueryable query)
            : base(query,
                  new PropertyQueryBuilder<TChangedBy, TChangeSource>(query),
                  new OperationTypeQueryBuilder<TChangedBy, TChangeSource>(query))
        {
        }

        public RootQueryBuilder(IQueryable query, ChangeEventConfiguration configuration)
            : base(query,
                  new PropertyQueryBuilder<TChangedBy, TChangeSource>(query),
                  new OperationTypeQueryBuilder<TChangedBy, TChangeSource>(query),
                  configuration)
        {
        }
    }
}