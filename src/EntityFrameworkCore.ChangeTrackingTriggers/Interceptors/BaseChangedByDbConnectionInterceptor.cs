using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
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
            var changedById = await changedByProvider.GetChangedById();

            await SetChangeTrackingContextAsync(eventData, changedById, cancellationToken);
        }

        protected abstract Task SetChangeTrackingContextAsync(ConnectionEndEventData eventData, TChangedBy changedBy, CancellationToken cancellationToken);
    }
}
