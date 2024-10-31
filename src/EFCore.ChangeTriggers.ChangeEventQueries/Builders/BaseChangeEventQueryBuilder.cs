using EFCore.ChangeTriggers.ChangeEventQueries.Builders.PropertyBuilders;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders
{
    internal abstract class BaseChangeEventQueryBuilder<TChangeEvent>
        where TChangeEvent : ChangeEvent
    {
        private readonly ChangeEventQueryConfiguration configuration;
        private readonly Type elementType;
        private readonly IChangeEventQueryPropertyBuilder<TChangeEvent> propertyBuilder;

        public BaseChangeEventQueryBuilder(
            ChangeEventQueryConfiguration configuration,
            Type elementType,
            IChangeEventQueryPropertyBuilder<TChangeEvent> propertyBuilder)
        {
            this.configuration = configuration;
            this.elementType = elementType;
            this.propertyBuilder = propertyBuilder;
        }

        public IQueryable<TChangeEvent> BuildChangeEventQuery()
        {
            var valueSelectors = configuration.Configurations[elementType];
            return valueSelectors.Select(propertyBuilder.BuildChangeEventQuery).Aggregate(Queryable.Union);
        }
    }
}