﻿using Microsoft.EntityFrameworkCore;
using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.ChangeEventQueries.EntityBuilder;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builder
{
    /// <inheritdoc/>
    /// <typeparam name="TChangedBy">The type that represents who a change was made by.</typeparam>
    /// <typeparam name="TChangeSource">The type that represents where a change originated from.</typeparam>
    public class ChangeEventQueryBuilder<TChangedBy, TChangeSource>
        : BaseChangeEventQueryBuilder<ChangeEvent<TChangedBy, TChangeSource>>
    {
        public ChangeEventQueryBuilder(DbContext context) : base(context)
        {
        }

        /// <summary>
        /// Adds the <typeparamref name="TChange"/> change entities to the builder.
        /// </summary>
        /// <typeparam name="TChange">The change entity to add to the builder.</typeparam>
        /// <param name="changes">The <see cref="IQueryable{T}"/> that contains the change entities to be built.</param>
        /// <param name="entityQueryBuilder">An action to perform operations on the entity query builder for the change entity.</param>
        /// <returns>The same query builder so further calls can be chained.</returns>
        public ChangeEventQueryBuilder<TChangedBy, TChangeSource> AddChanges<TChange>(
            IQueryable<TChange> changes,
            Action<IChangeEventEntityQueryBuilder<TChange, ChangeEvent<TChangedBy, TChangeSource>>> entityQueryBuilder)
            where TChange : IChange, IHasChangedBy<TChangedBy>, IHasChangeSource<TChangeSource>
        {
            var entityBuilderInstance = new ChangeEventEntityQueryBuilder<TChange, TChangedBy, TChangeSource>(context, changes);
            entityQueryBuilder(entityBuilderInstance);

            AddChanges(entityBuilderInstance);

            return this;
        }
    }
}
