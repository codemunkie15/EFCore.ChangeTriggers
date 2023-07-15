using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Interceptors
{
    internal abstract class BaseChangeSourceDbConnectionInterceptor<TChangeSource> : DbConnectionInterceptor
    {
        private readonly IChangeSourceProvider<TChangeSource> changeTrackingSourceTypeProvider;

        public BaseChangeSourceDbConnectionInterceptor(
            IChangeSourceProvider<TChangeSource> changeTrackingSourceTypeProvider)
        {
            this.changeTrackingSourceTypeProvider = changeTrackingSourceTypeProvider;
        }

        public override async Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = new())
        {
            var sourceType = await changeTrackingSourceTypeProvider.GetChangeSourceAsync();

            await SetChangeSourceChangeTrackingContextAsync(eventData, sourceType, cancellationToken);
        }

        protected abstract Task SetChangeSourceChangeTrackingContextAsync(ConnectionEndEventData eventData, TChangeSource changeSource, CancellationToken cancellationToken);
    }
}
