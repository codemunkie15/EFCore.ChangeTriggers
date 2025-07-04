using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Constants;
using EFCore.ChangeTriggers.Infrastructure;
using EFCore.ChangeTriggers.Interceptors;
using EFCore.ChangeTriggers.SqlServer.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EFCore.ChangeTriggers.SqlServer.Interceptors
{
    internal class ChangedBySqlServerDbConnectionInterceptor<TChangedBy> : BaseChangedByDbConnectionInterceptor<TChangedBy>
    {
        public ChangedBySqlServerDbConnectionInterceptor(
            ChangeTriggersExtensionContext changeTriggersExtensionContext,
            IChangedByProvider<TChangedBy> changedByProvider)
            : base(
                  changeTriggersExtensionContext,
                  changedByProvider)
        {
        }

        protected override void SetChangedByChangeContext(
            ConnectionEndEventData eventData,
            object? changedByProviderValue)
        {
            if (eventData.Connection.IsMasterDatabase())
            {
                // Database is probably being created, so don't set session context.
                return;
            }

            eventData.Context!.Database.ExecuteSql(
                $"EXEC sp_set_session_context {ChangeContextConstants.ChangedByContextName}, {changedByProviderValue}");
        }

        protected override async Task SetChangedByChangeContextAsync(
            ConnectionEndEventData eventData,
            object? changedByProviderValue,
            CancellationToken cancellationToken)
        {
            if (eventData.Connection.IsMasterDatabase())
            {
                // Database is probably being created, so don't set session context.
                return;
            }

            await eventData.Context!.Database.ExecuteSqlAsync(
                $"EXEC sp_set_session_context {ChangeContextConstants.ChangedByContextName}, {changedByProviderValue}",
                cancellationToken);
        }
    }
}
