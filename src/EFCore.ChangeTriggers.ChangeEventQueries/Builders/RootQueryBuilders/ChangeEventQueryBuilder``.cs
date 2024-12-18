using EFCore.ChangeTriggers.ChangeEventQueries.Builders.OperationTypeBuilders;
using EFCore.ChangeTriggers.ChangeEventQueries.Builders.PropertyBuilders;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.RootQueryBuilders
{
    internal class ChangeEventQueryBuilder<TChangedBy, TChangeSource>
        : BaseChangeEventQueryBuilder<ChangeEvent<TChangedBy, TChangeSource>>
    {
        public ChangeEventQueryBuilder(IQueryable query)
            : base(query,
                  new ChangeEventQueryPropertyBuilder<TChangedBy, TChangeSource>(query),
                  new ChangeEventQueryOperationTypeBuilder<TChangedBy, TChangeSource>(query))
        {
        }

        public ChangeEventQueryBuilder(IQueryable query, ChangeEventConfiguration configuration)
            : base(query,
                  new ChangeEventQueryPropertyBuilder<TChangedBy, TChangeSource>(query),
                  new ChangeEventQueryOperationTypeBuilder<TChangedBy, TChangeSource>(query),
                  configuration)
        {
        }
    }
}