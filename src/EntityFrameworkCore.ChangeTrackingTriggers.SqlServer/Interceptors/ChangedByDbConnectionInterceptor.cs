using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using EntityFrameworkCore.ChangeTrackingTriggers.Constants;
using EntityFrameworkCore.ChangeTrackingTriggers.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ChangeTrackingTriggers.SqlServer.Interceptors
{
    internal class ChangedByDbConnectionInterceptor<TChangedBy> : BaseChangedByDbConnectionInterceptor<TChangedBy>
    {
        public ChangedByDbConnectionInterceptor(
            IChangedByProvider<TChangedBy> changedByProvider)
            : base(changedByProvider)
        {
        }

        protected override async Task SetChangedByChangeTrackingContextAsync(
            ConnectionEndEventData eventData,
            object? changedByRawValue,
            CancellationToken cancellationToken)
        {
            var changedByProviderValue = eventData.Context?.ConvertToProvider<TChangedBy>(changedByRawValue);

            await eventData.Context!.Database.ExecuteSqlAsync(
                $"EXEC sp_set_session_context {ChangeTrackingContextConstants.ChangedByContextName}, {changedByProviderValue}",
                cancellationToken);
        }
    }
}
