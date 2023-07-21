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
            var changeSource = await GetChangeSourceAsync(eventData.Context);

            await SetChangeSourceChangeTrackingContextAsync(eventData, changeSource, cancellationToken);
        }

        protected abstract Task SetChangeSourceChangeTrackingContextAsync(ConnectionEndEventData eventData, object? changeSource, CancellationToken cancellationToken);

        private async Task<object?> GetChangeSourceAsync(DbContext context)
        {
            var changeSource = await changeTrackingChangeSourceProvider.GetChangeSourceAsync();

            var changeSourceEntityType = context.Model.FindEntityType(typeof(TChangeSource));

            if (changeSourceEntityType is not null)
            {
                // Use the ChangeSource entity primary key
                var primaryKeyProperty = changeSourceEntityType.GetSinglePrimaryKeyProperty();
                return primaryKeyProperty.GetGetter().GetClrValue(changeSource);
            }
            else
            {
                // Use the literal value
                return changeSource;
            }
        }
    }
}
