using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Metadata;

namespace EFCore.ChangeTriggers.ChangeEventQueries.EntityBuilder
{
    /// <summary>
    /// A query builder that queries <seealso cref="IChange"/> entities and transforms them into human readable change events.
    /// </summary>
    /// <typeparam name="TChangeEvent">The change event that will be built.</typeparam>
    /// <typeparam name="TChange">The change entity type.</typeparam>
    public abstract class BaseChangeEventEntityQueryBuilder<TChange, TChangeEvent>
        : IChangeEventEntityQueryBuilder<TChange, TChangeEvent>
        where TChange : IChange
    {
        private readonly DbContext context;
        private readonly IQueryable<TChange> dbSet;
        private List<IQueryable<TChangeEvent>> changeQueries = new();
        private Expression<Func<TChange, TChange, bool>>? cachedTrackedEntityForeignKeysAreEqualExpression;

        public BaseChangeEventEntityQueryBuilder(DbContext context, IQueryable<TChange> dbSet)
        {
            this.context = context;
            this.dbSet = dbSet;
        }

        /// <summary>
        /// Adds a change entity property to the builder.
        /// </summary>
        /// <param name="description">A human readable description of the property.</param>
        /// <param name="valueSelector">A function to select the property value as a string.</param>
        /// <returns>The same entity query builder so further calls can be chained.</returns>
        public IChangeEventEntityQueryBuilder<TChange, TChangeEvent> AddProperty(
            string description,
            Expression<Func<TChange, string>> valueSelector)
        {
            var trackedTablePrimaryKeysAreEqual = GetTrackedEntityForeignKeysAreEqualExpression();

            var baseQuery =
                from c in dbSet.AsExpandable()
                from pc in dbSet.AsExpandable()
                    .Where(pc => trackedTablePrimaryKeysAreEqual.Invoke(pc, c) && pc.ChangedAt < c.ChangedAt)
                    .OrderByDescending(pc => pc.ChangedAt)
                    .Take(1)
                where !valueSelector.Invoke(pc).Equals(valueSelector.Invoke(c)) // TODO: Should this compare on the string value or original column value?
                select new JoinedChanges<TChange>
                {
                    ChangeEntity = c,
                    PreviousChangeEntity = pc
                };

            var query = ProjectToResult(baseQuery, description, valueSelector);

            changeQueries.Add(query);

            return this;
        }

        /// <summary>
        /// Adds all the properties on the change entity to the builder.
        /// </summary>
        /// <remarks>Excludes primary keys, shadow properties and navigation properties.</remarks>
        /// <param name="descriptionBuilder">An optional function to format the description of properties.</param>
        /// <returns>The same entity query builder so further calls can be chained.</returns>
        public IChangeEventEntityQueryBuilder<TChange, TChangeEvent> AddEntityProperties(
            Func<string, string>? descriptionBuilder = null)
        {
            var entityPropertyNames = GetEntityPropertyNames();

            foreach (var propertyName in entityPropertyNames)
            {
                var expression = GetDefaultValueSelectorExpression(propertyName);
                var description = descriptionBuilder is not null ? descriptionBuilder(propertyName) : $"{propertyName} changed";

                AddProperty($"{description}", expression);
            }

            return this;
        }

        /// <summary>
        /// Projects the current and previous change entities into a property change event with an old and new value.
        /// </summary>
        /// <param name="query">The query to project.</param>
        /// <param name="description">The description to set for the property.</param>
        /// <param name="valueSelector">A function to select the property value as a string.</param>
        /// <returns>The change event query.</returns>
        protected abstract IQueryable<TChangeEvent> ProjectToResult(IQueryable<JoinedChanges<TChange>> query, string description, Expression<Func<TChange, string>> valueSelector);

        /// <summary>
        /// Builds the query and returns an <see cref="IQueryable{T}"/>.
        /// </summary>
        /// <returns>The built <see cref="IQueryable{T}"/>.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public IQueryable<TChangeEvent> Build()
        {
            if (changeQueries.Count == 0)
            {
                throw new InvalidOperationException("There are no queries configured to build.");
            }

            return changeQueries.Aggregate(Queryable.Concat);
        }

        private Expression<Func<TChange, TChange, bool>> GetTrackedEntityForeignKeysAreEqualExpression()
        {
            if (cachedTrackedEntityForeignKeysAreEqualExpression == null)
            {
                var changeEntityType = context.Model.FindEntityType(typeof(TChange))!;
                var trackedEntityForeignKeyProperties = changeEntityType.GetTrackedEntityForeignKey().Properties;

                var currentChangeParam = Expression.Parameter(typeof(TChange));
                var previousChangeParam = Expression.Parameter(typeof(TChange));

                var expressions = new List<Expression>();

                foreach (var foreignKeyProperty in trackedEntityForeignKeyProperties)
                {
                    var propertyName = foreignKeyProperty.Name;
                    var currentChangeProperty = Expression.PropertyOrField(currentChangeParam, propertyName);
                    var previousChangeProperty = Expression.PropertyOrField(previousChangeParam, propertyName);

                    var equal = Expression.Equal(currentChangeProperty, previousChangeProperty);
                    expressions.Add(equal);
                }

                var aggregatedExpression = expressions.Aggregate(Expression.AndAlso);
                cachedTrackedEntityForeignKeysAreEqualExpression = Expression.Lambda<Func<TChange, TChange, bool>>(
                    aggregatedExpression, currentChangeParam, previousChangeParam);
            }

            return cachedTrackedEntityForeignKeysAreEqualExpression;
        }

        private IEnumerable<string> GetEntityPropertyNames()
        {
            var changeEntity = context.Model.FindEntityType(typeof(TChange))!;
            var trackedEntityForeignKeyNames = changeEntity.GetTrackedEntityForeignKey().Properties.Select(p => p.Name);

            return changeEntity.GetProperties().Where(p =>
                !p.IsPrimaryKey() &&
                !p.IsShadowProperty() &&
                !p.IsChangeContextProperty() &&
                !trackedEntityForeignKeyNames.Contains(p.Name))
                .Select(p => p.Name);
        }

        private Expression<Func<TChange, string>> GetDefaultValueSelectorExpression(string propertyName)
        {
            var changeParameter = Expression.Parameter(typeof(TChange));
            var changeProperty = Expression.PropertyOrField(changeParameter, propertyName);

            // Convert to string
            var changePropertyAsString = Expression.Call(changeProperty, nameof(ToString), Type.EmptyTypes);

            return Expression.Lambda<Func<TChange, string>>(changePropertyAsString, changeParameter);
        }
    }
}
