using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.ChangeEventQueries.EntityBuilder;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builder
{
    /// <inheritdoc/>
    /// <typeparam name="TChangeSource">The type that represents where a change originated from.</typeparam>
    public class ChangeSourceChangeEventQueryBuilder<TChangeSource>
        : BaseChangeEventQueryBuilder<ChangeSourceChangeEvent<TChangeSource>>
    {
        public ChangeSourceChangeEventQueryBuilder(DbContext context) : base(context)
        {
        }

        /// <summary>
        /// Adds the <typeparamref name="TChange"/> change entities to the builder.
        /// </summary>
        /// <typeparam name="TChange">The change entity to add to the builder.</typeparam>
        /// <param name="changes">The <see cref="IQueryable{T}"/> that contains the change entities to be built.</param>
        /// <param name="entityQueryBuilder">An action to perform operations on the entity query builder for the change entity.</param>
        /// <returns>The same query builder so further calls can be chained.</returns>
        public ChangeSourceChangeEventQueryBuilder<TChangeSource> AddChanges<TChange>(
            IQueryable<TChange> changes,
            Action<IChangeEventEntityQueryBuilder<TChange, ChangeSourceChangeEvent<TChangeSource>>> entityQueryBuilder)
            where TChange : IChange, IHasChangeSource<TChangeSource>
        {
            var entityBuilderInstance = new ChangeSourceChangeEventEntityQueryBuilder<TChange, TChangeSource>(context, changes);
            entityQueryBuilder(entityBuilderInstance);

            AddChanges(entityBuilderInstance);

            return this;
        }
    }
}
