using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Constants;
using EFCore.ChangeTriggers.Infrastructure;
using EFCore.ChangeTriggers.Interceptors;
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
            eventData.Context!.Database.ExecuteSql(
                $"EXEC sp_set_session_context {ChangeContextConstants.ChangedByContextName}, {changedByProviderValue}");
        }

        protected override async Task SetChangedByChangeContextAsync(
            ConnectionEndEventData eventData,
            object? changedByProviderValue,
            CancellationToken cancellationToken)
        {
            await eventData.Context!.Database.ExecuteSqlAsync(
                $"EXEC sp_set_session_context {ChangeContextConstants.ChangedByContextName}, {changedByProviderValue}",
                cancellationToken);
        }
    }
}
