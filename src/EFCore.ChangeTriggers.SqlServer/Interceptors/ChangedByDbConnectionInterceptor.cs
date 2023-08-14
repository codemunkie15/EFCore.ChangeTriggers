using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Constants;
using EFCore.ChangeTriggers.EfCoreExtension;
using EFCore.ChangeTriggers.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EFCore.ChangeTriggers.SqlServer.Interceptors
{
    internal class ChangedByDbConnectionInterceptor<TChangedBy> : BaseChangedByDbConnectionInterceptor<TChangedBy>
    {
        public ChangedByDbConnectionInterceptor(
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
