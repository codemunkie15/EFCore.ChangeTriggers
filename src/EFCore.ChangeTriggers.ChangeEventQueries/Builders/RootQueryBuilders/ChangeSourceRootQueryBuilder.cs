using EFCore.ChangeTriggers.ChangeEventQueries.Builders.OperationTypeBuilders;
using EFCore.ChangeTriggers.ChangeEventQueries.Builders.PropertyBuilders;
using EFCore.ChangeTriggers.ChangeEventQueries.ChangeSource;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.RootQueryBuilders
{
    internal class ChangeSourceRootQueryBuilder<TChangeSource>
        : BaseRootQueryBuilder<ChangeEvent<TChangeSource>>
    {
        public ChangeSourceRootQueryBuilder(IQueryable query, ChangeEventConfiguration configuration)
            : base(query,
                  new ChangeSourcePropertyQueryBuilder<TChangeSource>(query),
                  new ChangeSourceOperationTypeQueryBuilder<TChangeSource>(query),
                  configuration)
        {
        }

        public ChangeSourceRootQueryBuilder(IQueryable query)
            : base(query,
                  new ChangeSourcePropertyQueryBuilder<TChangeSource>(query),
                  new ChangeSourceOperationTypeQueryBuilder<TChangeSource>(query))
        {
        }
    }
}