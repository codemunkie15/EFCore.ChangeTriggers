using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.ChangeEventQueries.Extensions;
using System.Linq.Expressions;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.OperationTypeBuilders
{
    internal abstract class BaseChangeEventQueryOperationTypeBuilder<TChangeEvent> : BuilderBase<TChangeEvent>, IChangeEventQueryOperationTypeBuilder<TChangeEvent>
        where TChangeEvent : ChangeEvent
    {
        private readonly IQueryable query;
        private ParameterExpression c;

        public BaseChangeEventQueryOperationTypeBuilder(IQueryable query)
        {
            this.query = query;

            c = Expression.Parameter(query.ElementType, "c");
        }

        /// <![CDATA[
        /// this.query
        ///     .Where(c => c.OperationType == operationType)
        ///     .Select(c => new ChangeEvent
        ///     {
        ///         ChangedAt = c.ChangedAt,
        ///         Description = description
        ///     });
        public IQueryable<TChangeEvent> Build(OperationType operationType)
        {
            var operationTypeProperty = Expression.Property(c, nameof(IChange.OperationType));
            var operationTypeConstant = Expression.Constant(operationType);
            var whereCondition = Expression.Equal(operationTypeProperty, operationTypeConstant);

            var filteredExpression = query.Expression.ApplyWhere(whereCondition, c);
            var projectedExpression = BuildChangeEventProjection(filteredExpression, operationType);

            return query.Provider.CreateQuery<TChangeEvent>(projectedExpression);
        }

        private MethodCallExpression BuildChangeEventProjection(
            Expression expression,
            OperationType operationType)
        {
            var bindings = new[]
            {
                BuildChangeEventPropertyBinding(ce => ce.Description, Expression.Constant(GetDescription(operationType))),
                BuildChangeEventPropertyBinding(ce => ce.ChangedAt, Expression.Property(c, nameof(IChange.ChangedAt))),
                BuildChangeEventPropertyBinding(ce => ce.OldValue, Expression.Constant(string.Empty)),
                BuildChangeEventPropertyBinding(ce => ce.NewValue, Expression.Constant(string.Empty)),
            }.Concat(GetAdditionalChangeEventPropertyBindings(c));

            var changeEventInit = Expression.MemberInit(Expression.New(typeof(TChangeEvent)), bindings);

            return expression.ApplySelect(changeEventInit, c);
        }

        private string GetDescription(OperationType operationType)
        {
            switch (operationType)
            {
                case OperationType.Insert:
                    return "Entity created";
                case OperationType.Delete:
                    return "Entity deleted";
                default:
                    throw new ArgumentOutOfRangeException($"The builder does not support the operation type {operationType}.");
            }
        }
    }
}