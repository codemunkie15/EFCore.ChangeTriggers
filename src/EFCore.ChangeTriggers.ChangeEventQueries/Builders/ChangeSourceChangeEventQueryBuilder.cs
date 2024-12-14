using EFCore.ChangeTriggers.ChangeEventQueries.Builders.PropertyBuilders;
using EFCore.ChangeTriggers.ChangeEventQueries.ChangeSource;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders
{
    internal class ChangeSourceChangeEventQueryBuilder<TChangeSource>
        : BaseChangeEventQueryBuilder<ChangeEvent<TChangeSource>>
    {
        public ChangeSourceChangeEventQueryBuilder(IQueryable query, ChangeEventConfiguration configuration)
            : base(query, new ChangeSourceChangeEventQueryPropertyBuilder<TChangeSource>(query), configuration)
        {
        }

        public ChangeSourceChangeEventQueryBuilder(IQueryable query)
            : base(query, new ChangeSourceChangeEventQueryPropertyBuilder<TChangeSource>(query))
        {
        }
    }
}