using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using EntityFrameworkCore.ChangeTrackingTriggers.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Interceptors
{
    internal abstract class BaseChangeSourceDbConnectionInterceptor<TChangeSource> : DbConnectionInterceptor
    {
        private readonly IChangeSourceProvider<TChangeSource> changeTrackingChangeSourceProvider;

        public BaseChangeSourceDbConnectionInterceptor(
            IChangeSourceProvider<TChangeSource> changeTrackingSourceTypeProvider)
        {
            this.changeTrackingChangeSourceProvider = changeTrackingSourceTypeProvider;
        }

        public override async Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = new())
        {
            var changeSource = await changeTrackingChangeSourceProvider.GetChangeSourceAsync();
            var changeSourceRawValue = eventData.Context?.Model.GetRawValue<TChangeSource>(changeSource);

            await SetChangeSourceChangeTrackingContextAsync(eventData, changeSourceRawValue, cancellationToken);
        }

        protected abstract Task SetChangeSourceChangeTrackingContextAsync(ConnectionEndEventData eventData, object? changeSourceRawValue, CancellationToken cancellationToken);
    }
}
