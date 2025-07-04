using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.ChangeEventQueries.Builders.OperationTypeBuilders;
using EFCore.ChangeTriggers.ChangeEventQueries.Builders.PropertyBuilders;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;
using EFCore.ChangeTriggers.ChangeEventQueries.Exceptions;
using EFCore.ChangeTriggers.ChangeEventQueries.Extensions;
using EFCore.ChangeTriggers.ChangeEventQueries.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.RootQueryBuilders
{
    internal abstract class BaseRootQueryBuilder<TChangeEvent>
        where TChangeEvent : ChangeEvent
    {
        private readonly ChangeEventsDbContextOptionsExtension options;
        private readonly ChangeEventConfiguration configuration;
        private readonly IQueryable query;
        private readonly IPropertyQueryBuilder<TChangeEvent> propertyBuilder;
        private readonly IOperationTypeQueryBuilder<TChangeEvent> operationTypeBuilder;

        public BaseRootQueryBuilder(
            IQueryable query,
            IPropertyQueryBuilder<TChangeEvent> propertyBuilder,
            IOperationTypeQueryBuilder<TChangeEvent> operationTypeBuilder)
            : this(query, propertyBuilder, operationTypeBuilder, GetConfigurationFromDbContext(query))
        {
        }

        public BaseRootQueryBuilder(
            IQueryable query,
            IPropertyQueryBuilder<TChangeEvent> propertyBuilder,
            IOperationTypeQueryBuilder<TChangeEvent> operationTypeBuilder,
            ChangeEventConfiguration configuration)
        {
            query.EnsureElementType<IChange>();

            this.options = query.GetDbContext().GetService<IDbContextOptions>().FindExtension<ChangeEventsDbContextOptionsExtension>()
                ?? throw new ChangeEventQueryException(ExceptionStrings.ChangeEventsDbContextOptionsExtensionNotFound());
            this.query = query;
            this.propertyBuilder = propertyBuilder;
            this.operationTypeBuilder = operationTypeBuilder;
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IQueryable<TChangeEvent> Build()
        {
            var entityConfiguration = configuration.EntityConfigurations.GetValueOrDefault(query.ElementType)
                ?? throw new ChangeEventQueryException(ExceptionStrings.EntityConfigurationNotFound(query.ElementType.Name));

            IQueryable<TChangeEvent>? changeEventsQuery = null;

            // Add property queries
            foreach (var propertyConfig in entityConfiguration.PropertyConfigurations)
            {
                var propertyQuery = propertyBuilder.Build(propertyConfig);
                changeEventsQuery = changeEventsQuery == null
                    ? propertyQuery
                    : changeEventsQuery.Union(propertyQuery);
            }

            // Add inserts
            if (options.IncludeInserts || entityConfiguration.AddInserts)
            {
                var insertQuery = operationTypeBuilder.Build(OperationType.Insert);
                changeEventsQuery = changeEventsQuery == null
                    ? insertQuery
                    : insertQuery.Union(changeEventsQuery);
            }

            // Add deletes
            if (options.IncludeDeletes || entityConfiguration.AddDeletes)
            {
                var deleteQuery = operationTypeBuilder.Build(OperationType.Delete);
                changeEventsQuery = changeEventsQuery == null
                    ? deleteQuery
                    : changeEventsQuery.Union(deleteQuery);
            }

            return changeEventsQuery
                ?? throw new Exception("Change this");
        }

        private static ChangeEventConfiguration GetConfigurationFromDbContext(IQueryable query)
        {
            return query.GetDbContext().GetInfrastructure().GetService<ChangeEventConfiguration>()
                ?? throw new ChangeEventQueryException(ExceptionStrings.ConfigurationNotFoundFromDbContext());
        }
    }
}