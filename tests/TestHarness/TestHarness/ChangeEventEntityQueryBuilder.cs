using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LinqKit;

namespace TestHarness
{
    internal class ChangeEventEntityQueryBuilder<TChange, TTracked, TChangeId>
        where TChange : class, IChange<TTracked, TChangeId>
    {
        private readonly DbContext context;
        private readonly IQueryable<TChange> dbSet;
        private List<IQueryable<ChangeEvent>> changeQueries = new();

        public ChangeEventEntityQueryBuilder(DbContext context, IQueryable<TChange> dbSet)
        {
            this.context = context;
            this.dbSet = dbSet;
        }

        public ChangeEventEntityQueryBuilder<TChange, TTracked, TChangeId> AddProperty(
            string description,
            Expression<Func<TChange, string>> valueSelector)
        {
            var primaryKeysAreEqual = GetPrimaryKeysAreEqualExpression();

            var query =
                from uc in dbSet.AsExpandable()
                from puc in dbSet.AsExpandable()
                    .Where(puc => primaryKeysAreEqual.Invoke(puc, uc) && puc.ChangedAt < uc.ChangedAt)
                    .OrderByDescending(puc => puc.ChangedAt)
                    .Take(1)
                where !valueSelector.Invoke(puc).Equals(valueSelector.Invoke(uc))
                select new ChangeEvent
                {
                    Description = description,
                    OldValue = valueSelector.Invoke(puc),
                    NewValue = valueSelector.Invoke(uc),
                    ChangedAt = uc.ChangedAt
                };

            changeQueries.Add(query);

            return this;
        }

        public IQueryable<ChangeEvent> Build()
        {
            if (!changeQueries.Any())
            {
                throw new InvalidOperationException("There are no queries configured to build.");
            }

            IQueryable<ChangeEvent>? final = null;
            foreach (var query in changeQueries)
            {
                if (final == null)
                {
                    final = query;
                }
                else
                {
                    final = final.Union(query);
                }
            }

            return final!;
        }

        private Expression<Func<TChange, TChange, bool>> GetPrimaryKeysAreEqualExpression()
        {
            var changeEntityType = context.Model.FindEntityType(typeof(TTracked));
            var primaryKeyProperties = changeEntityType.FindPrimaryKey().Properties;

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

        //private Expression<Func<TChange, TChange, bool>> GetChangePropertiesAreNotEqualExpression<TChangeProperty>(
        //    Expression<Func<TChange, TChangeProperty>> getChangeProperty)
        //{
        //    var currentChangeParam = Expression.Parameter(typeof(TChange));
        //    var previousChangeParam = Expression.Parameter(typeof(TChange));

        //    var currentGetChangePropertyExpression = Expression.Invoke(getChangeProperty, currentChangeParam);
        //    var previousGetChangePropertyExpression = Expression.Invoke(getChangeProperty, previousChangeParam);

        //    var notEqual = Expression.NotEqual(currentGetChangePropertyExpression, previousGetChangePropertyExpression);
        //    return Expression.Lambda<Func<TChange, TChange, bool>>(notEqual, currentChangeParam, previousChangeParam);
        //}
    }
}
