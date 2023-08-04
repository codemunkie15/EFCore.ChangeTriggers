using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Constants;
using EFCore.ChangeTriggers.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EFCore.ChangeTriggers.SqlServer.Interceptors
{
    internal class ChangedByDbConnectionInterceptor<TChangedBy> : BaseChangedByDbConnectionInterceptor<TChangedBy>
    {
        public ChangedByDbConnectionInterceptor(
            IChangedByProvider<TChangedBy> changedByProvider)
            : base(changedByProvider)
        {
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
