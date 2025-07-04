using EFCore.ChangeTriggers.ChangeEventQueries.Builders.OperationTypeBuilders;
using EFCore.ChangeTriggers.ChangeEventQueries.Builders.PropertyBuilders;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.RootQueryBuilders
{
    internal class RootQueryBuilder
        : BaseRootQueryBuilder<ChangeEvent>
    {
        public RootQueryBuilder(IQueryable query)
            : base(query,
                  new PropertyQueryBuilder(query),
                  new OperationTypeQueryBuilder(query)
                  )
        {
        }

        public RootQueryBuilder(IQueryable query, ChangeEventConfiguration configuration)
            : base(query,
                  new PropertyQueryBuilder(query),
                  new OperationTypeQueryBuilder(query),
                  configuration)
        {
        }
    }
}