using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Infrastructure;
using EFCore.ChangeTriggers.Metadata;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace EFCore.ChangeTriggers.Interceptors
{
    internal abstract class BaseChangeSourceDbConnectionInterceptor<TChangeSource> : DbConnectionInterceptor
    {
        private readonly ChangeTriggersExtensionContext changeTriggersExtensionContext;
        private readonly IChangeSourceProvider<TChangeSource> changeSourceProvider;

        public BaseChangeSourceDbConnectionInterceptor(
            ChangeTriggersExtensionContext changeTriggersExtensionContext,
            IChangeSourceProvider<TChangeSource> changeSourceProvider)
        {
            this.changeTriggersExtensionContext = changeTriggersExtensionContext;
            this.changeSourceProvider = changeSourceProvider;
        }

        public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
        {
            var changeSource = changeTriggersExtensionContext.IsMigrationRunning
                ? changeSourceProvider.GetMigrationChangeSource()
                : changeSourceProvider.GetChangeSource();

            var changeSourceRawValue = eventData.Context?.Model.GetRawValue<TChangeSource>(changeSource);

            SetChangeSourceChangeContext(eventData, changeSourceRawValue);
        }

        public override async Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = new())
        {
            var changeSource = changeTriggersExtensionContext.IsMigrationRunning
                ? await changeSourceProvider.GetMigrationChangeSourceAsync()
                : await changeSourceProvider.GetChangeSourceAsync();

            var changeSourceRawValue = eventData.Context?.Model.GetRawValue<TChangeSource>(changeSource);

            await SetChangeSourceChangeContextAsync(eventData, changeSourceRawValue, cancellationToken);
        }

        protected abstract void SetChangeSourceChangeContext(ConnectionEndEventData eventData, object? changeSourceRawValue);

        protected abstract Task SetChangeSourceChangeContextAsync(ConnectionEndEventData eventData, object? changeSourceRawValue, CancellationToken cancellationToken);
    }
}
