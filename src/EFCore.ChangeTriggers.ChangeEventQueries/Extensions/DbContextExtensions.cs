using Microsoft.EntityFrameworkCore;
using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.ChangeEventQueries.Builder;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Extensions
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// Creates a query builder that queries <seealso cref="IChange"/> entities and transforms them into human readable change events.
        /// </summary>
        /// <typeparam name="TChangedBy">The type that represents who a change was made by.</typeparam>
        /// <typeparam name="TChangeSource">The type that represents where a change originated from.</typeparam>
        /// <param name="context">The DbContext to use.</param>
        /// <returns>The query builder to use.</returns>
        public static ChangeEventQueryBuilder<TChangedBy, TChangeSource> CreateChangeEventQueryBuilder<TChangedBy, TChangeSource>(this DbContext context)
        {
            return new ChangeEventQueryBuilder<TChangedBy, TChangeSource>(context);
        }

        /// <summary>
        /// Creates a query builder that queries <seealso cref="IChange"/> entities and transforms them into human readable change events.
        /// </summary>
        /// <typeparam name="TChangedBy">The type that represents who a change was made by.</typeparam>
        /// <param name="context">The DbContext to use.</param>
        /// <returns>The query builder to use.</returns>
        public static ChangedByChangeEventQueryBuilder<TChangedBy> CreateChangedByChangeEventQueryBuilder<TChangedBy>(this DbContext context)
        {
            return new ChangedByChangeEventQueryBuilder<TChangedBy>(context);
        }

        /// <summary>
        /// Creates a query builder that queries <seealso cref="IChange"/> entities and transforms them into human readable change events.
        /// </summary>
        /// <typeparam name="TChangeSource">The type that represents where a change originated from.</typeparam>
        /// <param name="context">The DbContext to use.</param>
        /// <returns>The query builder to use.</returns>
        public static ChangeSourceChangeEventQueryBuilder<TChangeSource> CreateChangeSourceChangeEventQueryBuilder<TChangeSource>(this DbContext context)
        {
            return new ChangeSourceChangeEventQueryBuilder<TChangeSource>(context);
        }

        /// <summary>
        /// Creates a query builder that queries <seealso cref="IChange"/> entities and transforms them into human readable change events.
        /// </summary>
        /// <param name="context">The DbContext to use.</param>
        /// <returns>The query builder to use.</returns>
        public static ChangeEventQueryBuilder CreateChangeEventQueryBuilder(this DbContext context)
        {
            return new ChangeEventQueryBuilder(context);
        }
    }
}
