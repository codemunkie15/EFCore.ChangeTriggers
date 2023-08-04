using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using EntityFrameworkCore.ChangeTrackingTriggers.ChangeEventQueries.EntityBuilder;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ChangeTrackingTriggers.ChangeEventQueries.Builder
{
    /// <inheritdoc/>
    /// <typeparam name="TChangedBy">The type that represents who a change was made by.</typeparam>
    public class ChangedByChangeEventQueryBuilder<TChangedBy>
        : BaseChangeEventQueryBuilder<ChangedByChangeEvent<TChangedBy>>
    {
        public ChangedByChangeEventQueryBuilder(DbContext context) : base(context)
        {
        }

        /// <summary>
        /// Adds the <typeparamref name="TChange"/> change entities to the builder.
        /// </summary>
        /// <typeparam name="TChange">The change entity to add to the builder.</typeparam>
        /// <param name="changes">The <see cref="IQueryable{T}"/> that contains the change entities to be built.</param>
        /// <param name="entityQueryBuilder">An action to perform operations on the entity query builder for the change entity.</param>
        /// <returns>The same query builder so further calls can be chained.</returns>
        public ChangedByChangeEventQueryBuilder<TChangedBy> AddChanges<TChange>(
            IQueryable<TChange> changes,
            Action<IChangeEventEntityQueryBuilder<TChange, ChangedByChangeEvent<TChangedBy>>> entityQueryBuilder)
            where TChange : IChange, IHasChangedBy<TChangedBy>
        {
            var entityBuilderInstance = new ChangedByChangeEventEntityQueryBuilder<TChange, TChangedBy>(context, changes);
            entityQueryBuilder(entityBuilderInstance);

            AddChanges(entityBuilderInstance);

            return this;
        }
    }
}
