using Microsoft.EntityFrameworkCore;
using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.ChangeEventQueries.EntityBuilder;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builder
{
    /// <summary>
    /// A query builder that queries <seealso cref="IChange"/> entities and transforms them into human readable change events.
    /// </summary>
    /// <typeparam name="TChangeEvent">The change event that will be built.</typeparam>
    public abstract class BaseChangeEventQueryBuilder<TChangeEvent>
    {
        protected readonly DbContext context;
        private List<IQueryable<TChangeEvent>> changeQueries = new();

        public BaseChangeEventQueryBuilder(DbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Builds the query and returns an <see cref="IQueryable{T}"/>.
        /// </summary>
        /// <returns>The built <see cref="IQueryable{T}"/>.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public IQueryable<TChangeEvent> Build()
        {
            if (!changeQueries.Any())
            {
                throw new InvalidOperationException("There are no queries configured to build.");
            }

            return changeQueries.Aggregate(Queryable.Concat);
        }

        /// <summary>
        /// Builds the <paramref name="entityQueryBuilder"/> and adds the <see cref="IQueryable{T}"/> to this query builder.
        /// </summary>
        /// <typeparam name="TChange">The change entity to add to the builder.</typeparam>
        /// <param name="entityQueryBuilder">The entity query builder to build and add to this query builder.</param>
        protected void AddChanges<TChange>(IChangeEventEntityQueryBuilder<TChange, TChangeEvent> entityQueryBuilder)
        {
            changeQueries.Add(entityQueryBuilder.Build());
        }
    }
}
