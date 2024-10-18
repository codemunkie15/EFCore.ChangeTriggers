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
            object? changedByRawValue)
        {
            var changedByProviderValue = eventData.Context?.ConvertToProvider<TChangedBy>(changedByRawValue);

            eventData.Context!.Database.ExecuteSql(
                $"EXEC sp_set_session_context {ChangeContextConstants.ChangedByContextName}, {changedByProviderValue}");
        }

        protected override async Task SetChangedByChangeContextAsync(
            ConnectionEndEventData eventData,
            object? changedByRawValue,
            CancellationToken cancellationToken)
        {
            var changedByProviderValue = eventData.Context?.ConvertToProvider<TChangedBy>(changedByRawValue);

            await eventData.Context!.Database.ExecuteSqlAsync(
                $"EXEC sp_set_session_context {ChangeContextConstants.ChangedByContextName}, {changedByProviderValue}",
                cancellationToken);
        }
    }
}
