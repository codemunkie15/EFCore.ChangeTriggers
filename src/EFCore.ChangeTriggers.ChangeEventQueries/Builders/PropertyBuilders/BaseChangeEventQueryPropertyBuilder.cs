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
        private readonly DbContext dbContext;

        public BaseChangeEventQueryPropertyBuilder(IQueryable query)
        {
            query.EnsureElementType<IChange>();

            this.query = query;
            dbContext = GetDbContext(query);
        }

        public IQueryable<TChangeEvent> BuildChangeEventQuery(LambdaExpression valueSelector)
        {
            var selectManyExpression = BuildSelectManyExpression();
            var filteredExpression = ApplyFilterToJoinedChanges(selectManyExpression, valueSelector);
            var finalSelectExpression = BuildChangeEventProjection(filteredExpression, valueSelector);

            return query.Provider.CreateQuery<TChangeEvent>(finalSelectExpression);
        }

        protected virtual IEnumerable<MemberBinding> GetAdditionalChangeEventPropertyBindings(MemberExpression changeEntity) => [];

        protected MemberBinding BuildChangeEventPropertyBinding<TResult>(Expression<Func<TChangeEvent, TResult>> memberExpression, Expression selector)
        {
            var memberInfo = ((MemberExpression)memberExpression.Body).Member;
            return Expression.Bind(memberInfo, selector);
        }

        private DbContext GetDbContext(IQueryable query)
        {
            if (query is IInfrastructure<IServiceProvider> infrastructure)
            {
                var dbContextServices = infrastructure.Instance.GetRequiredService<IDbContextServices>();
                return dbContextServices.CurrentContext.Context;
            }

            throw new ChangeEventQueryException("The provided query is not an EF Core queryable.");
        }

        private Expression BuildSelectManyExpression()
        {
            var changedAtProperty = query.ElementType.GetProperty(nameof(IChange.ChangedAt))!;
            var c = Expression.Parameter(query.ElementType, "c");
            var pc = Expression.Parameter(query.ElementType, "pc");

            var foreignKeyEqualityCondition = BuildTrackedEntityForeignKeysAreEqualExpression(c, pc);
            var whereCondition = Expression.LessThan(
                Expression.Property(pc, changedAtProperty),
                Expression.Property(c, changedAtProperty));

            var combinedCondition = Expression.AndAlso(whereCondition, foreignKeyEqualityCondition);
            var innerWhere = ApplyWhere(query.Expression, combinedCondition, pc);

            var innerOrderBy = ApplyOrderByDescending(innerWhere, changedAtProperty, pc);
            var innerTake = ApplyTake(innerOrderBy, 1);

            return BuildSelectMany(c, innerTake);
        }

        private Expression BuildTrackedEntityForeignKeysAreEqualExpression(ParameterExpression c, ParameterExpression pc)
        {
            var changeEntityType = dbContext.Model.FindEntityType(query.ElementType)!;

            return changeEntityType
                .GetTrackedEntityForeignKey()
                .Properties
                .Select(prop => Expression.Equal(
                    Expression.Property(c, prop.PropertyInfo!),
                    Expression.Property(pc, prop.Name)))
                .Aggregate(Expression.AndAlso);
        }

        private Expression ApplyFilterToJoinedChanges(Expression selectManyExpression, LambdaExpression valueSelector)
        {
            var joinedChangesType = typeof(JoinedChanges<>).MakeGenericType(query.ElementType);
            var jcParameter = Expression.Parameter(joinedChangesType, "jc");

            var jccProperty = Expression.Property(jcParameter, nameof(JoinedChanges<object>.ChangeEntity));
            var jcpcProperty = Expression.Property(jcParameter, nameof(JoinedChanges<object>.PreviousChangeEntity));

            var cValueSelector = new ParameterReplaceVisitor(valueSelector.Parameters[0], jccProperty).Visit(valueSelector.Body);
            var pcValueSelector = new ParameterReplaceVisitor(valueSelector.Parameters[0], jcpcProperty).Visit(valueSelector.Body);

            var outerWhere = Expression.Lambda(Expression.NotEqual(cValueSelector, pcValueSelector), jcParameter);
            return ApplyWhere(selectManyExpression, outerWhere.Body, jcParameter);
        }

        private Expression BuildChangeEventProjection(Expression filteredExpression, LambdaExpression valueSelector)
        {
            var joinedChangesType = typeof(JoinedChanges<>).MakeGenericType(query.ElementType);
            var jcParameter = Expression.Parameter(joinedChangesType, "jc");

            var jccProperty = Expression.Property(jcParameter, nameof(JoinedChanges<object>.ChangeEntity));
            var jcpcProperty = Expression.Property(jcParameter, nameof(JoinedChanges<object>.PreviousChangeEntity));

            var cValueSelector = new ParameterReplaceVisitor(valueSelector.Parameters[0], jccProperty).Visit(valueSelector.Body);
            var pcValueSelector = new ParameterReplaceVisitor(valueSelector.Parameters[0], jcpcProperty).Visit(valueSelector.Body);

            var bindings = new[]
            {
                BuildChangeEventPropertyBinding(ce => ce.ChangedAt, Expression.Property(jccProperty, nameof(IChange.ChangedAt))),
                BuildChangeEventPropertyBinding(ce => ce.OldValue, pcValueSelector),
                BuildChangeEventPropertyBinding(ce => ce.NewValue, cValueSelector)
            }.Concat(GetAdditionalChangeEventPropertyBindings(jccProperty));

            var changeEventInit = Expression.MemberInit(Expression.New(typeof(TChangeEvent)), bindings);

            return ApplySelect(filteredExpression, changeEventInit, jcParameter);
        }

        private Expression BuildSelectMany(ParameterExpression outerParameter, Expression innerExpression)
        {
            var joinedChangesType = typeof(JoinedChanges<>).MakeGenericType(query.ElementType);
            var joinedChangesConstructor = joinedChangesType.GetConstructor(Type.EmptyTypes)!;
            var collectionSelector = Expression.Lambda(
                typeof(Func<,>).MakeGenericType(query.ElementType, typeof(IEnumerable<>).MakeGenericType(query.ElementType)),
                innerExpression,
                outerParameter);

            var c = Expression.Parameter(query.ElementType, "c");
            var pc = Expression.Parameter(query.ElementType, "pc");
            var joinedChangesInit = Expression.MemberInit(
                Expression.New(joinedChangesConstructor),
                Expression.Bind(joinedChangesType.GetProperty(nameof(JoinedChanges<object>.ChangeEntity))!, c),
                Expression.Bind(joinedChangesType.GetProperty(nameof(JoinedChanges<object>.PreviousChangeEntity))!, pc)
            );

            var resultSelector = Expression.Lambda(joinedChangesInit, c, pc);

            return Expression.Call(
                typeof(Queryable),
                nameof(Queryable.SelectMany),
                [query.ElementType, query.ElementType, joinedChangesType],
                query.Expression,
                collectionSelector,
                resultSelector);
        }

        private Expression ApplySelect(Expression source, Expression selector, ParameterExpression parameter)
        {
            return Expression.Call(
                typeof(Queryable),
                nameof(Queryable.Select),
                [parameter.Type, selector.Type],
                source,
                Expression.Lambda(selector, parameter));
        }

        private Expression ApplyWhere(Expression source, Expression predicate, ParameterExpression parameter)
        {
            return Expression.Call(
                typeof(Queryable),
                nameof(Queryable.Where),
                [parameter.Type],
                source,
                Expression.Lambda(predicate, parameter));
        }

        private Expression ApplyOrderByDescending(Expression source, PropertyInfo property, ParameterExpression parameter)
        {
            return Expression.Call(
                typeof(Queryable),
                nameof(Queryable.OrderByDescending),
                [parameter.Type, typeof(DateTimeOffset)],
                source,
                Expression.Lambda(Expression.Property(parameter, property), parameter));
        }

        private Expression ApplyTake(Expression source, int count)
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
