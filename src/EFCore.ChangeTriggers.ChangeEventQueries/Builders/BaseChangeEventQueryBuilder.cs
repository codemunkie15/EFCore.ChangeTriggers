using EFCore.ChangeTriggers.ChangeEventQueries.Builders.PropertyBuilders;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;
using EFCore.ChangeTriggers.ChangeEventQueries.Extensions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders
{
    internal abstract class BaseChangeEventQueryBuilder<TChangeEvent>
        where TChangeEvent : ChangeEvent
    {
        private readonly ChangeEventConfiguration configuration;
        private readonly IQueryable query;
        private readonly IChangeEventQueryPropertyBuilder<TChangeEvent> propertyBuilder;

        public BaseChangeEventQueryBuilder(
            IQueryable query,
            IChangeEventQueryPropertyBuilder<TChangeEvent> propertyBuilder)
            : this(query, propertyBuilder, GetConfigurationFromDbContext(query))
        {
        }

        public BaseChangeEventQueryBuilder(
            IQueryable query,
            IChangeEventQueryPropertyBuilder<TChangeEvent> propertyBuilder,
            ChangeEventConfiguration configuration)
        {
            this.query = query;
            this.propertyBuilder = propertyBuilder;
            this.configuration = configuration;
        }

        public IQueryable<TChangeEvent> Build()
        {
            var entityConfiguration = configuration.EntityConfigurations[query.ElementType];
            return entityConfiguration.PropertyConfigurations
                .Select(propertyBuilder.Build)
                .Aggregate(Queryable.Union);
        }

        private static ChangeEventConfiguration GetConfigurationFromDbContext(IQueryable query)
        {
            return query.GetDbContext().GetInfrastructure().GetService<ChangeEventConfiguration>()
                ?? throw new Exception(); // TODO: Exception message
        }
    }
}