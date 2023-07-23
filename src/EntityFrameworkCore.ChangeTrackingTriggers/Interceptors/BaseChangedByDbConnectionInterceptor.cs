using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using EntityFrameworkCore.ChangeTrackingTriggers.Extensions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Interceptors
{
    internal abstract class BaseChangedByDbConnectionInterceptor<TChangedBy> : DbConnectionInterceptor
    {
        private readonly IChangedByProvider<TChangedBy> changedByProvider;

        public BaseChangedByDbConnectionInterceptor(
            IChangedByProvider<TChangedBy> changedByProvider)
        {
            this.changedByProvider = changedByProvider;
        }

        public override async Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = new())
        {
            var changedBy = await changedByProvider.GetChangedByAsync();
            var changedByRawValue = eventData.Context?.Model.GetRawValue<TChangedBy>(changedBy);

            await SetChangedByChangeTrackingContextAsync(eventData, changedByRawValue, cancellationToken);
        }

        protected abstract Task SetChangedByChangeTrackingContextAsync(ConnectionEndEventData eventData, object? changedByRawValue, CancellationToken cancellationToken);
    }
}
