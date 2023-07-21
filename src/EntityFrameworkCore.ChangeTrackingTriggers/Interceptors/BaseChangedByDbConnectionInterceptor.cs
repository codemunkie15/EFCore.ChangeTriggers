using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using EntityFrameworkCore.ChangeTrackingTriggers.Extensions;
using Microsoft.EntityFrameworkCore;
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
            var changedBy = await GetChangedByAsync(eventData.Context);

            await SetChangedByChangeTrackingContextAsync(eventData, changedBy, cancellationToken);
        }

        protected abstract Task SetChangedByChangeTrackingContextAsync(ConnectionEndEventData eventData, object? changedBy, CancellationToken cancellationToken);

        private async Task<object?> GetChangedByAsync(DbContext context)
        {
            var changedBy = await changedByProvider.GetChangedByAsync();

            var changedByEntityType = context.Model.FindEntityType(typeof(TChangedBy));

            if (changedByEntityType is not null)
            {
                // Use the ChangedBy entity primary key
                var primaryKeyProperty = changedByEntityType.GetSinglePrimaryKeyProperty();
                return primaryKeyProperty.GetGetter().GetClrValue(changedBy);
            }
            else
            {
                // Use the literal value
                return changedBy;
            }
        }
    }
}
