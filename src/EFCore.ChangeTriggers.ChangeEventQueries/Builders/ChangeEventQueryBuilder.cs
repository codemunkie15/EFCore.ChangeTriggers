using EFCore.ChangeTriggers.ChangeEventQueries.Builders.PropertyBuilders;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders
{
    internal class ChangeEventQueryBuilder
        : BaseChangeEventQueryBuilder<ChangeEvent>
    {
        public ChangeEventQueryBuilder(
            ChangeEventQueryConfiguration configuration,
            IQueryable query)
            : base(configuration, query.ElementType, new ChangeEventQueryPropertyBuilder(query))
        {
        }
    }
}