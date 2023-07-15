using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using EntityFrameworkCore.ChangeTrackingTriggers.Constants;
using EntityFrameworkCore.ChangeTrackingTriggers.Interceptors;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EntityFrameworkCore.ChangeTrackingTriggers.SqlServer.Interceptors
{
    internal class ChangeSourceDbConnectionInterceptor<TChangeSource> : BaseChangeSourceDbConnectionInterceptor<TChangeSource>
    {
        public ChangeSourceDbConnectionInterceptor(
            IChangeSourceProvider<TChangeSource> changeSourceTypeProvider)
            : base(changeSourceTypeProvider)
        {
        }

        protected override async Task SetChangeSourceChangeTrackingContextAsync(
            ConnectionEndEventData eventData,
            TChangeSource changeSource,
            CancellationToken cancellationToken)
        {
            var database = eventData.Context!.Database;

            var changeSourceParameter = new SqlParameter("changeSource", changeSource);

            await database.ExecuteSqlRawAsync(
                $"EXEC sp_set_session_context '{ChangeTrackingContextConstants.ChangeSourceContextName}', @changeSource",
                new[] { changeSourceParameter },
                cancellationToken);
        }
    }
}
