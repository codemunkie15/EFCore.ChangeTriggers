using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.ChangeEventQueries.Exceptions;
using EFCore.ChangeTriggers.ChangeEventQueries.Extensions;
using EFCore.ChangeTriggers.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Reflection;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.PropertyBuilders
{
    public abstract class BaseChangeEventQueryPropertyBuilder<TChangeEvent> : IChangeEventQueryPropertyBuilder<TChangeEvent>
        where TChangeEvent : ChangeEvent
    {
        private readonly IQueryable query;
        private readonly ChangePair<ParameterExpression> changeParams;
        private readonly Type cpGenericType;
        private readonly ParameterExpression cpJoinParam;
        private readonly ChangePair<MemberExpression> cpJoinProps;
        private readonly PropertyInfo changedAtProp;
        private readonly BinaryExpression foreignKeyEqualityCondition;

        public BaseChangeEventQueryPropertyBuilder(IQueryable query)
        {
            query.EnsureElementType<IChange>();

            this.query = query;

            changeParams = new()
            {
                Current = Expression.Parameter(query.ElementType, "c"),
                Previous = Expression.Parameter(query.ElementType, "pc")
            };

            cpGenericType = typeof(ChangePair<>).MakeGenericType(query.ElementType);
            cpJoinParam = Expression.Parameter(cpGenericType, "cp");
            cpJoinProps = new()
            {
                Current = Expression.Property(cpJoinParam, nameof(ChangePair<object>.Current)),
                Previous = Expression.Property(cpJoinParam, nameof(ChangePair<object>.Previous))
            };

            changedAtProp = query.ElementType.GetProperty(nameof(IChange.ChangedAt))!;
            foreignKeyEqualityCondition = BuildForeignKeyEqualityCondition();
        }

        public IQueryable<TChangeEvent> BuildChangeEventQuery(LambdaExpression valueSelector)
        {
            var selectors = new ChangePair<Expression>
            {
                Current = new ParameterReplaceVisitor(valueSelector.Parameters[0], cpJoinProps.Current).Visit(valueSelector.Body),
                Previous = new ParameterReplaceVisitor(valueSelector.Parameters[0], cpJoinProps.Previous).Visit(valueSelector.Body)
            };

            var selectManyExpression = BuildSelectManyExpression();
            var filteredExpression = BuildChangePairWhereClause(selectManyExpression, selectors);
            var finalSelectExpression = BuildChangeEventProjection(filteredExpression, selectors);

            return query.Provider.CreateQuery<TChangeEvent>(finalSelectExpression);
        }

        protected MemberBinding BuildChangeEventPropertyBinding<TResult>(Expression<Func<TChangeEvent, TResult>> memberExpression, Expression selector)
        {
            var memberInfo = ((MemberExpression)memberExpression.Body).Member;
            return Expression.Bind(memberInfo, selector);
        }

        protected virtual IEnumerable<MemberBinding> GetAdditionalChangeEventPropertyBindings(MemberExpression changeEntity) => [];

        private MethodCallExpression BuildSelectManyExpression()
        {
            var whereCondition = Expression.LessThan(
                Expression.Property(changeParams.Previous, changedAtProp),
                Expression.Property(changeParams.Current, changedAtProp));

            var combinedCondition = Expression.AndAlso(whereCondition, foreignKeyEqualityCondition);
            var innerWhere = ApplyWhere(query.Expression, combinedCondition, changeParams.Previous);

            var innerOrderBy = ApplyOrderByDescending(innerWhere, changedAtProp, changeParams.Previous);
            var innerTake = ApplyTake(innerOrderBy, 1);

            return BuildSelectMany(innerTake);
        }

        private MethodCallExpression BuildSelectMany(Expression innerExpression)
        {
            var cpConstructor = cpGenericType.GetConstructor(Type.EmptyTypes)!;
            var collectionSelector = Expression.Lambda(
                typeof(Func<,>).MakeGenericType(query.ElementType, typeof(IEnumerable<>).MakeGenericType(query.ElementType)),
                innerExpression,
                changeParams.Current);

            var joinedChangesInit = Expression.MemberInit(
                Expression.New(cpConstructor),
                Expression.Bind(cpGenericType.GetProperty(nameof(ChangePair<object>.Current))!, changeParams.Current),
                Expression.Bind(cpGenericType.GetProperty(nameof(ChangePair<object>.Previous))!, changeParams.Previous)
            );

            var resultSelector = Expression.Lambda(joinedChangesInit, changeParams.Current, changeParams.Previous);

            return Expression.Call(
                typeof(Queryable),
                nameof(Queryable.SelectMany),
                [query.ElementType, query.ElementType, cpGenericType],
                query.Expression,
                collectionSelector,
                resultSelector);
        }

        private BinaryExpression BuildForeignKeyEqualityCondition()
        {
            var dbContext = query.GetDbContext();
            var changeEntityType = dbContext.Model.FindEntityType(query.ElementType)!;

            return changeEntityType
                .GetTrackedEntityForeignKey()
                .Properties
                .Select(prop => Expression.Equal(
                    Expression.Property(changeParams.Current, prop.PropertyInfo!),
                    Expression.Property(changeParams.Previous, prop.Name)))
                .Aggregate(Expression.AndAlso);
        }

        private MethodCallExpression BuildChangePairWhereClause(Expression selectManyExpression, ChangePair<Expression> selectors)
        {
            var outerWhere = Expression.Lambda(Expression.NotEqual(selectors.Current, selectors.Previous), cpJoinParam);
            return ApplyWhere(selectManyExpression, outerWhere.Body, cpJoinParam);
        }

        private MethodCallExpression BuildChangeEventProjection(Expression filteredExpression, ChangePair<Expression> selectors)
        {
            var bindings = new[]
            {
                BuildChangeEventPropertyBinding(ce => ce.ChangedAt, Expression.Property(cpJoinProps.Current, nameof(IChange.ChangedAt))),
                BuildChangeEventPropertyBinding(ce => ce.OldValue, selectors.Previous),
                BuildChangeEventPropertyBinding(ce => ce.NewValue, selectors.Current)
            }.Concat(GetAdditionalChangeEventPropertyBindings(cpJoinProps.Current));

            var changeEventInit = Expression.MemberInit(Expression.New(typeof(TChangeEvent)), bindings);

            return ApplySelect(filteredExpression, changeEventInit, cpJoinParam);
        }

        private MethodCallExpression ApplySelect(Expression source, Expression selector, ParameterExpression parameter)
        {
            return Expression.Call(
                typeof(Queryable),
                nameof(Queryable.Select),
                [parameter.Type, selector.Type],
                source,
                Expression.Lambda(selector, parameter));
        }

        private MethodCallExpression ApplyWhere(Expression source, Expression predicate, ParameterExpression parameter)
        {
            return Expression.Call(
                typeof(Queryable),
                nameof(Queryable.Where),
                [parameter.Type],
                source,
                Expression.Lambda(predicate, parameter));
        }

        private MethodCallExpression ApplyOrderByDescending(Expression source, PropertyInfo property, ParameterExpression parameter)
        {
            return Expression.Call(
                typeof(Queryable),
                nameof(Queryable.OrderByDescending),
                [parameter.Type, typeof(DateTimeOffset)],
                source,
                Expression.Lambda(Expression.Property(parameter, property), parameter));
        }

        private MethodCallExpression ApplyTake(Expression source, int count)
        {
            return Expression.Call(
                typeof(Queryable),
                nameof(Queryable.Take),
                [query.ElementType],
                source,
                Expression.Constant(count));
        }
    }
}
