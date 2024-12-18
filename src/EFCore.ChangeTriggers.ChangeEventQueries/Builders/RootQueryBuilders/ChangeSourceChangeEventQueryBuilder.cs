using EFCore.ChangeTriggers.ChangeEventQueries.Builders.OperationTypeBuilders;
using EFCore.ChangeTriggers.ChangeEventQueries.Builders.PropertyBuilders;
using EFCore.ChangeTriggers.ChangeEventQueries.ChangeSource;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.RootQueryBuilders
{
    internal class ChangeSourceChangeEventQueryBuilder<TChangeSource>
        : BaseChangeEventQueryBuilder<ChangeEvent<TChangeSource>>
    {
        public ChangeSourceChangeEventQueryBuilder(IQueryable query, ChangeEventConfiguration configuration)
            : base(query,
                  new ChangeSourceChangeEventQueryPropertyBuilder<TChangeSource>(query),
                  new ChangeSourceChangeEventQueryOperationTypeBuilder<TChangeSource>(query),
                  configuration)
        {
        }

        public ChangeSourceChangeEventQueryBuilder(IQueryable query)
            : base(query,
                  new ChangeSourceChangeEventQueryPropertyBuilder<TChangeSource>(query),
                  new ChangeSourceChangeEventQueryOperationTypeBuilder<TChangeSource>(query))
        {
        }
    }
}