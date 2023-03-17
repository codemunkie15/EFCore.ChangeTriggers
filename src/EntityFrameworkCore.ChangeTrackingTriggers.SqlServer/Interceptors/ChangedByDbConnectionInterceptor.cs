using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using EntityFrameworkCore.ChangeTrackingTriggers.Constants;
using EntityFrameworkCore.ChangeTrackingTriggers.Interceptors;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EntityFrameworkCore.ChangeTrackingTriggers.SqlServer.Interceptors
{
    internal class ChangedByDbConnectionInterceptor<TChangedBy> : BaseChangedByDbConnectionInterceptor<TChangedBy>
    {
        public ChangedByDbConnectionInterceptor(
            IChangedByProvider<TChangedBy> changedByProvider)
            : base(changedByProvider)
        {
        }

        protected override async Task SetChangeTrackingContextAsync(
            ConnectionEndEventData eventData,
            TChangedBy changedBy,
            CancellationToken cancellationToken)
        {
            var database = eventData.Context!.Database;

            var userIdParameter = new SqlParameter("changedBy", changedBy);

            await database.ExecuteSqlRawAsync(
                $"EXEC sp_set_session_context '{ChangeTrackingContextConstants.ChangedByContextName}', @changedBy",
                new[] { userIdParameter },
                cancellationToken);
        }
    }
}
