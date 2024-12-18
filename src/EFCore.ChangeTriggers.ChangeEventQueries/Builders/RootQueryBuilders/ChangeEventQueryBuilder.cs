using EFCore.ChangeTriggers.ChangeEventQueries.Builders.OperationTypeBuilders;
using EFCore.ChangeTriggers.ChangeEventQueries.Builders.PropertyBuilders;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.RootQueryBuilders
{
    internal class ChangeEventQueryBuilder
        : BaseChangeEventQueryBuilder<ChangeEvent>
    {
        public ChangeEventQueryBuilder(IQueryable query)
            : base(query,
                  new ChangeEventQueryPropertyBuilder(query),
                  new ChangeEventQueryOperationTypeBuilder(query)
                  )
        {
        }

        public ChangeEventQueryBuilder(IQueryable query, ChangeEventConfiguration configuration)
            : base(query,
                  new ChangeEventQueryPropertyBuilder(query),
                  new ChangeEventQueryOperationTypeBuilder(query),
                  configuration)
        {
        }
    }
}