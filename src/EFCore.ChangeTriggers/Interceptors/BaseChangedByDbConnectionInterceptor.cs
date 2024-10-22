using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Infrastructure;
using EFCore.ChangeTriggers.Metadata;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace EFCore.ChangeTriggers.Interceptors
{
    internal abstract class BaseChangedByDbConnectionInterceptor<TChangedBy> : DbConnectionInterceptor
    {
        private readonly ChangeTriggersExtensionContext changeTriggersExtensionContext;
        private readonly IChangedByProvider<TChangedBy> changedByProvider;

        public BaseChangedByDbConnectionInterceptor(
            ChangeTriggersExtensionContext changeTriggersExtensionContext,
            IChangedByProvider<TChangedBy> changedByProvider)
        {
            this.changeTriggersExtensionContext = changeTriggersExtensionContext;
            this.changedByProvider = changedByProvider;
        }

        public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
        {
            var changedBy = changeTriggersExtensionContext.IsMigrationRunning
                ? changedByProvider.GetMigrationChangedBy()
                : changedByProvider.GetChangedBy();

            var changedByRawValue = eventData.Context?.Model.GetRawValue<TChangedBy>(changedBy);

            SetChangedByChangeContext(eventData, changedByRawValue);
        }

        public override async Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = new())
        {
            var changedBy = changeTriggersExtensionContext.IsMigrationRunning
                ? await changedByProvider.GetMigrationChangedByAsync()
                : await changedByProvider.GetChangedByAsync();

            var changedByRawValue = eventData.Context?.Model.GetRawValue<TChangedBy>(changedBy);

            await SetChangedByChangeContextAsync(eventData, changedByRawValue, cancellationToken);
        }

        protected abstract void SetChangedByChangeContext(ConnectionEndEventData eventData, object? changedByRawValue);

        protected abstract Task SetChangedByChangeContextAsync(ConnectionEndEventData eventData, object? changedByRawValue, CancellationToken cancellationToken);
    }
}
