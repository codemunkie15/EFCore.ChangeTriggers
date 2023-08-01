using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using EntityFrameworkCore.ChangeTrackingTriggers.Extensions;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Queries.EntityBuilder
{
    public abstract class BaseChangeEventEntityQueryBuilder<TChangeEvent, TChange>
        : IChangeEventEntityQueryBuilder<TChange, TChangeEvent>
        where TChange : IChange
    {
        private readonly DbContext context;
        private readonly IQueryable<TChange> dbSet;
        private List<IQueryable<TChangeEvent>> changeQueries = new();

        public BaseChangeEventEntityQueryBuilder(DbContext context, IQueryable<TChange> dbSet)
        {
            this.context = context;
            this.dbSet = dbSet;
        }

        public IChangeEventEntityQueryBuilder<TChange, TChangeEvent> AddProperty(
            string description,
            Expression<Func<TChange, string>> valueSelector)
        {
            var trackedTablePrimaryKeysAreEqual = GetTrackedTablePrimaryKeysAreEqualExpression();

            var baseQuery =
                from uc in dbSet.AsExpandable()
                from puc in dbSet.AsExpandable()
                    .Where(puc => trackedTablePrimaryKeysAreEqual.Invoke(puc, uc) && puc.ChangedAt < uc.ChangedAt)
                    .OrderByDescending(puc => puc.ChangedAt)
                    .Take(1)
                where !valueSelector.Invoke(puc).Equals(valueSelector.Invoke(uc)) // TODO: Should this compare on the string value or original column value?
                select new JoinedChanges<TChange>
                {
                    ChangeEntity = uc,
                    PreviousChangeEntity = puc
                };

            var query = ProjectToResult(baseQuery, description, valueSelector);

            changeQueries.Add(query);

            return this;
        }

        protected abstract IQueryable<TChangeEvent> ProjectToResult(IQueryable<JoinedChanges<TChange>> query, string description, Expression<Func<TChange, string>> valueSelector);

        public IQueryable<TChangeEvent> Build()
        {
            if (!changeQueries.Any())
            {
                throw new InvalidOperationException("There are no queries configured to build.");
            }

            return changeQueries.Aggregate(Queryable.Concat);
        }

        private Expression<Func<TChange, TChange, bool>> GetTrackedTablePrimaryKeysAreEqualExpression()
        {
            var changeEntityType = context.Model.FindEntityType(typeof(TChange))!;
            var trackedEntityType = changeEntityType.GetTrackedEntityType();

            var primaryKeyProperties = trackedEntityType.FindPrimaryKey().Properties;

            var currentChangeParam = Expression.Parameter(typeof(TChange));
            var previousChangeParam = Expression.Parameter(typeof(TChange));

            var expressions = new List<Expression>();

            foreach (var primaryKeyProperty in primaryKeyProperties)
            {
                var propertyName = primaryKeyProperty.Name;
                var currentChangeProperty = Expression.PropertyOrField(currentChangeParam, propertyName);
                var previousChangeProperty = Expression.PropertyOrField(previousChangeParam, propertyName);

                var equal = Expression.Equal(currentChangeProperty, previousChangeProperty);
                expressions.Add(equal);
            }

            var aggregatedExpression = expressions.Aggregate(Expression.AndAlso);
            return Expression.Lambda<Func<TChange, TChange, bool>>(aggregatedExpression, currentChangeParam, previousChangeParam);
        }
    }
}
