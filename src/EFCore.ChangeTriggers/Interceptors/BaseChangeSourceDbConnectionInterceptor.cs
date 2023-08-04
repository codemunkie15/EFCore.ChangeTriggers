using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Extensions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace EFCore.ChangeTriggers.Interceptors
{
    internal abstract class BaseChangeSourceDbConnectionInterceptor<TChangeSource> : DbConnectionInterceptor
    {
        private readonly IChangeSourceProvider<TChangeSource> changeSourceProvider;

        public BaseChangeSourceDbConnectionInterceptor(
            IChangeSourceProvider<TChangeSource> changeSourceProvider)
        {
            this.changeSourceProvider = changeSourceProvider;
        }

        public override async Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = new())
        {
            var changeSource = await changeSourceProvider.GetChangeSourceAsync();
            var changeSourceRawValue = eventData.Context?.Model.GetRawValue<TChangeSource>(changeSource);

            await SetChangeSourceChangeContextAsync(eventData, changeSourceRawValue, cancellationToken);
        }

        protected abstract Task SetChangeSourceChangeContextAsync(ConnectionEndEventData eventData, object? changeSourceRawValue, CancellationToken cancellationToken);
    }
}
