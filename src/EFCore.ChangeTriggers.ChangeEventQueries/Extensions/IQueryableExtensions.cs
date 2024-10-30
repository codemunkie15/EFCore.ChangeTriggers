using EFCore.ChangeTriggers.ChangeEventQueries.Builders;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace EFCore.ChangeTriggers.ChangeEventQueries
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    public static class IQueryableExtensions
    {
        public static IQueryable<ChangeEvent> ToChangeEvents(this IQueryable query, ChangeEventQueryConfiguration configuration)
        {
            var builder = new ChangeEventQueryBuilder(configuration, query);
            return builder.BuildChangeEventQuery();
        }

        public static IQueryable<ChangeEvent<TChangedBy, TChangeSource>> ToChangeEvents<TChangedBy, TChangeSource>(this IQueryable query, ChangeEventQueryConfiguration configuration)
        {
            var builder = new ChangeEventQueryBuilder<TChangedBy, TChangeSource>(configuration, query);
            return builder.BuildChangeEventQuery();
        }
    }
}

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace EFCore.ChangeTriggers.ChangeEventQueries.ChangedBy
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    public static class IQueryableExtensions
    {

        public static IQueryable<ChangeEvent<TChangedBy>> ToChangeEvents<TChangedBy>(this IQueryable query, ChangeEventQueryConfiguration configuration)
        {
            var builder = new ChangedByChangeEventQueryBuilder<TChangedBy>(configuration, query);
            return builder.BuildChangeEventQuery();
        }
    }
}

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace EFCore.ChangeTriggers.ChangeEventQueries.ChangeSource
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    public static class IQueryableExtensions
    {

        public static IQueryable<ChangeEvent<TChangeSource>> ToChangeEvents<TChangeSource>(this IQueryable query, ChangeEventQueryConfiguration configuration)
        {
            var builder = new ChangeSourceChangeEventQueryBuilder<TChangeSource>(configuration, query);
            return builder.BuildChangeEventQuery();
        }
    }
}