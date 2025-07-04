using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Exceptions;
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
            if (eventData.Context == null)
            {
                throw new ChangeTriggersException(ExceptionStrings.InterceptorDbContextNotSet);
            }

            var changeSource = changeTriggersExtensionContext.IsMigrationRunning
                ? changeSourceProvider.GetMigrationChangeSource()
                : changeSourceProvider.GetChangeSource();

            var changeSourceRawValue = eventData.Context.Model.GetRawValue(changeSource);
            var changeSourceProviderValue = eventData.Context.Model.ConvertToProvider(changeSourceRawValue);

            SetChangeSourceChangeContext(eventData, changeSourceProviderValue);
        }

        public override async Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = new())
        {
            if (eventData.Context == null)
            {
                throw new ChangeTriggersException(ExceptionStrings.InterceptorDbContextNotSet);
            }

            var changeSource = changeTriggersExtensionContext.IsMigrationRunning
                ? await changeSourceProvider.GetMigrationChangeSourceAsync()
                : await changeSourceProvider.GetChangeSourceAsync();

            var changeSourceRawValue = eventData.Context.Model.GetRawValue(changeSource);
            var changeSourceProviderValue = eventData.Context.Model.ConvertToProvider(changeSourceRawValue);

            await SetChangeSourceChangeContextAsync(eventData, changeSourceProviderValue, cancellationToken);
        }

        protected abstract void SetChangeSourceChangeContext(ConnectionEndEventData eventData, object? changeSourceProviderValue);

        protected abstract Task SetChangeSourceChangeContextAsync(ConnectionEndEventData eventData, object? changeSourceProviderValue, CancellationToken cancellationToken);
    }
}
