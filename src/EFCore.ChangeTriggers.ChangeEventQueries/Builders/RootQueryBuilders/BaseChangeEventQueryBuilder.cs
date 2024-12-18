using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.ChangeEventQueries.Builders.OperationTypeBuilders;
using EFCore.ChangeTriggers.ChangeEventQueries.Builders.PropertyBuilders;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;
using EFCore.ChangeTriggers.ChangeEventQueries.Exceptions;
using EFCore.ChangeTriggers.ChangeEventQueries.Extensions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.RootQueryBuilders
{
    internal abstract class BaseChangeEventQueryBuilder<TChangeEvent>
        where TChangeEvent : ChangeEvent
    {
        private readonly ChangeEventConfiguration configuration;
        private readonly IQueryable query;
        private readonly IChangeEventQueryPropertyBuilder<TChangeEvent> propertyBuilder;
        private readonly IChangeEventQueryOperationTypeBuilder<TChangeEvent> operationTypeBuilder;

        public BaseChangeEventQueryBuilder(
            IQueryable query,
            IChangeEventQueryPropertyBuilder<TChangeEvent> propertyBuilder,
            IChangeEventQueryOperationTypeBuilder<TChangeEvent> operationTypeBuilder)
            : this(query, propertyBuilder, operationTypeBuilder, GetConfigurationFromDbContext(query))
        {
        }

        public BaseChangeEventQueryBuilder(
            IQueryable query,
            IChangeEventQueryPropertyBuilder<TChangeEvent> propertyBuilder,
            IChangeEventQueryOperationTypeBuilder<TChangeEvent> operationTypeBuilder,
            ChangeEventConfiguration configuration)
        {
            query.EnsureElementType<IChange>();

            this.query = query;
            this.propertyBuilder = propertyBuilder;
            this.operationTypeBuilder = operationTypeBuilder;
            this.configuration = configuration;
        }

        public IQueryable<TChangeEvent> Build()
        {
            var entityConfiguration = configuration.EntityConfigurations[query.ElementType];

            var changeEventsQuery = entityConfiguration.PropertyConfigurations
                .Select(propertyBuilder.Build)
                .Aggregate(Queryable.Union);

            if (entityConfiguration.AddInserts)
            {
                var insertQuery = operationTypeBuilder.Build(OperationType.Insert);
                changeEventsQuery = insertQuery.Union(changeEventsQuery);
            }

            if (entityConfiguration.AddDeletes)
            {
                var deleteQuery = operationTypeBuilder.Build(OperationType.Delete);
                changeEventsQuery = changeEventsQuery.Union(deleteQuery);
            }

            return changeEventsQuery;
        }

        private static ChangeEventConfiguration GetConfigurationFromDbContext(IQueryable query)
        {
            return query.GetDbContext().GetInfrastructure().GetService<ChangeEventConfiguration>()
                ?? throw new ChangeEventQueryException(ExceptionStrings.ConfigurationNotFound(query.ElementType.Name));
        }
    }
}