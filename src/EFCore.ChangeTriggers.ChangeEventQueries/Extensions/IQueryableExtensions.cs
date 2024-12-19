using EFCore.ChangeTriggers.ChangeEventQueries.Builders.RootQueryBuilders;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace EFCore.ChangeTriggers.ChangeEventQueries
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    public static class IQueryableExtensions
    {
        public static IQueryable<ChangeEvent> ToChangeEvents(this IQueryable query)
        {
            return new RootQueryBuilder(query).Build();
        }

        public static IQueryable<ChangeEvent> ToChangeEvents(this IQueryable query, ChangeEventConfiguration configuration)
        {
            return new RootQueryBuilder(query, configuration).Build();
        }

        public static IQueryable<ChangeEvent<TChangedBy, TChangeSource>> ToChangeEvents<TChangedBy, TChangeSource>(this IQueryable query)
        {
            return new RootQueryBuilder<TChangedBy, TChangeSource>(query).Build();
        }

        public static IQueryable<ChangeEvent<TChangedBy, TChangeSource>> ToChangeEvents<TChangedBy, TChangeSource>(this IQueryable query, ChangeEventConfiguration configuration)
        {
            return new RootQueryBuilder<TChangedBy, TChangeSource>(query, configuration).Build();
        }
    }
}

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace EFCore.ChangeTriggers.ChangeEventQueries.ChangedBy
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    public static class IQueryableExtensions
    {

        public static IQueryable<ChangeEvent<TChangedBy>> ToChangeEvents<TChangedBy>(this IQueryable query, ChangeEventConfiguration configuration)
        {
            return new ChangedByRootQueryBuilder<TChangedBy>(query, configuration).Build();
        }
    }
}

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace EFCore.ChangeTriggers.ChangeEventQueries.ChangeSource
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    public static class IQueryableExtensions
    {

        public static IQueryable<ChangeEvent<TChangeSource>> ToChangeEvents<TChangeSource>(this IQueryable query, ChangeEventConfiguration configuration)
        {
            return new ChangeSourceRootQueryBuilder<TChangeSource>(query, configuration).Build();
        }
    }
}