using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Infrastructure;
using EFCore.ChangeTriggers.Metadata;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Diagnostics;

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
            Debugger.Launch();

            if (eventData.Context == null)
            {
                // TODO: what exception?
                throw new Exception("");
            }

            var changedBy = changeTriggersExtensionContext.IsMigrationRunning
                ? changedByProvider.GetMigrationChangedBy()
                : changedByProvider.GetChangedBy();

            var changedByRawValue = eventData.Context.Model.GetRawValue(changedBy); // TODO: Is there a better way to get this?
            var changedByProviderValue = eventData.Context.Model.ConvertToProvider(changedByRawValue);

            SetChangedByChangeContext(eventData, changedByProviderValue);
        }

        public override async Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = new())
        {
            if (eventData.Context == null)
            {
                // TODO: what exception?
                throw new Exception("");
            }

            var changedBy = changeTriggersExtensionContext.IsMigrationRunning
                ? await changedByProvider.GetMigrationChangedByAsync()
                : await changedByProvider.GetChangedByAsync();

            var changedByRawValue = eventData.Context.Model.GetRawValue(changedBy);
            var changedByProviderValue = eventData.Context.Model.ConvertToProvider(changedByRawValue);

            await SetChangedByChangeContextAsync(eventData, changedByProviderValue, cancellationToken);
        }

        protected abstract void SetChangedByChangeContext(ConnectionEndEventData eventData, object? changedByProviderValue);

        protected abstract Task SetChangedByChangeContextAsync(ConnectionEndEventData eventData, object? changedByProviderValue, CancellationToken cancellationToken);
    }
}
