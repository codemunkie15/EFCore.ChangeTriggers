using EntityFrameworkCore.ChangeTrackingTriggers.Queries.Builder;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Queries
{
    public static class DbContextExtensions
    {
        public static ChangeEventQueryBuilder<TChangedBy, TChangeSource> CreateChangeEventQueryBuilder<TChangedBy, TChangeSource>(this DbContext context)
        {
            return new ChangeEventQueryBuilder<TChangedBy, TChangeSource>(context);
        }

        public static ChangedByChangeEventQueryBuilder<TChangedBy> CreateChangedByChangeEventQueryBuilder<TChangedBy>(this DbContext context)
        {
            return new ChangedByChangeEventQueryBuilder<TChangedBy>(context);
        }

        public static ChangeSourceChangeEventQueryBuilder<TChangeSource> CreateChangeSourceChangeEventQueryBuilder<TChangeSource>(this DbContext context)
        {
            return new ChangeSourceChangeEventQueryBuilder<TChangeSource>(context);
        }

        public static ChangeEventQueryBuilder CreateChangeEventQueryBuilder(this DbContext context)
        {
            return new ChangeEventQueryBuilder(context);
        }
    }
}
