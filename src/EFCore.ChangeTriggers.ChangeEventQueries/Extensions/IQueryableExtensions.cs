using EFCore.ChangeTriggers.ChangeEventQueries.Builders.RootQueryBuilders;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace EFCore.ChangeTriggers.ChangeEventQueries
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    public static class IQueryableExtensions
    {
        /// <summary>
        /// Projects change entities into human readable change events.
        /// </summary>
        /// <param name="query">The EF core queryable to project.</param>
        /// <returns>The built change event query.</returns>
        public static IQueryable<ChangeEvent> ToChangeEvents(this IQueryable query)
        {
            return new RootQueryBuilder(query).Build();
        }

        /// <summary>
        /// Projects change entities into human readable change events, overriding any previous configuration.
        /// </summary>
        /// <param name="query">The EF core queryable to project.</param>
        /// <param name="configuration">The configuration to use for this query.</param>
        /// <returns>The built change event query.</returns>
        public static IQueryable<ChangeEvent> ToChangeEvents(this IQueryable query, ChangeEventConfiguration configuration)
        {
            return new RootQueryBuilder(query, configuration).Build();
        }

        /// <summary>
        /// Projects change entities into human readable change events.
        /// </summary>
        /// <param name="query">The EF core queryable to project.</param>
        /// <returns>The built change event query.</returns>
        public static IQueryable<ChangeEvent<TChangedBy, TChangeSource>> ToChangeEvents<TChangedBy, TChangeSource>(this IQueryable query)
        {
            return new RootQueryBuilder<TChangedBy, TChangeSource>(query).Build();
        }

        /// <summary>
        /// Projects change entities into human readable change events, overriding any previous configuration.
        /// </summary>
        /// <param name="query">The EF core queryable to project.</param>
        /// <param name="configuration">The configuration to use for this query.</param>
        /// <returns>The built change event query.</returns>
        public static IQueryable<ChangeEvent<TChangedBy, TChangeSource>> ToChangeEvents<TChangedBy, TChangeSource>(this IQueryable query, ChangeEventConfiguration configuration)
        {
            return new RootQueryBuilder<TChangedBy, TChangeSource>(query, configuration).Build();
        }
    }
}

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace EFCore.ChangeTriggers.ChangeEventQueries.ChangedByEvents
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    public static class IQueryableExtensions
    {
        /// <summary>
        /// Projects change entities into human readable change events.
        /// </summary>
        /// <param name="query">The EF core queryable to project.</param>
        /// <returns>The built change event query.</returns>
        public static IQueryable<ChangeEvent<TChangedBy>> ToChangeEvents<TChangedBy>(this IQueryable query)
        {
            return new ChangedByRootQueryBuilder<TChangedBy>(query).Build();
        }

        /// <summary>
        /// Projects change entities into human readable change events, overriding any previous configuration.
        /// </summary>
        /// <param name="query">The EF core queryable to project.</param>
        /// <param name="configuration">The configuration to use for this query.</param>
        /// <returns>The built change event query.</returns>
        public static IQueryable<ChangeEvent<TChangedBy>> ToChangeEvents<TChangedBy>(this IQueryable query, ChangeEventConfiguration configuration)
        {
            return new ChangedByRootQueryBuilder<TChangedBy>(query, configuration).Build();
        }
    }
}

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace EFCore.ChangeTriggers.ChangeEventQueries.ChangeSourceEvents
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    public static class IQueryableExtensions
    {
        /// <summary>
        /// Projects change entities into human readable change events.
        /// </summary>
        /// <param name="query">The EF core queryable to project.</param>
        /// <returns>The built change event query.</returns>
        public static IQueryable<ChangeEvent<TChangeSource>> ToChangeEvents<TChangeSource>(this IQueryable query)
        {
            return new ChangeSourceRootQueryBuilder<TChangeSource>(query).Build();
        }

        /// <summary>
        /// Projects change entities into human readable change events, overriding any previous configuration.
        /// </summary>
        /// <param name="query">The EF core queryable to project.</param>
        /// <param name="configuration">The configuration to use for this query.</param>
        /// <returns>The built change event query.</returns>
        public static IQueryable<ChangeEvent<TChangeSource>> ToChangeEvents<TChangeSource>(this IQueryable query, ChangeEventConfiguration configuration)
        {
            return new ChangeSourceRootQueryBuilder<TChangeSource>(query, configuration).Build();
        }
    }
}