using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Extensions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace EFCore.ChangeTriggers.Interceptors
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
            var changedBy = await changedByProvider.GetChangedByAsync();
            var changedByRawValue = eventData.Context?.Model.GetRawValue<TChangedBy>(changedBy);

            await SetChangedByChangeContextAsync(eventData, changedByRawValue, cancellationToken);
        }

        protected abstract Task SetChangedByChangeContextAsync(ConnectionEndEventData eventData, object? changedByRawValue, CancellationToken cancellationToken);
    }
}
