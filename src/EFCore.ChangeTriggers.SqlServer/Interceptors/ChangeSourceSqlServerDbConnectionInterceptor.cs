using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Constants;
using EFCore.ChangeTriggers.Infrastructure;
using EFCore.ChangeTriggers.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EFCore.ChangeTriggers.SqlServer.Interceptors
{
    internal class ChangeSourceSqlServerDbConnectionInterceptor<TChangeSource> : BaseChangeSourceDbConnectionInterceptor<TChangeSource>
    {
        public ChangeSourceSqlServerDbConnectionInterceptor(
            ChangeTriggersExtensionContext changeTriggersExtensionContext,
            IChangeSourceProvider<TChangeSource> changeSourceProvider)
            : base(
                  changeTriggersExtensionContext,
                  changeSourceProvider)
        {
        }

        protected override void SetChangeSourceChangeContext(
            ConnectionEndEventData eventData,
            object? changeSourceProviderValue)
        {
            eventData.Context!.Database.ExecuteSql(
                $"EXEC sp_set_session_context {ChangeContextConstants.ChangeSourceContextName}, {changeSourceProviderValue}");
        }

        protected override async Task SetChangeSourceChangeContextAsync(
            ConnectionEndEventData eventData,
            object? changeSourceProviderValue,
            CancellationToken cancellationToken)
        {
            await eventData.Context!.Database.ExecuteSqlAsync(
                $"EXEC sp_set_session_context {ChangeContextConstants.ChangeSourceContextName}, {changeSourceProviderValue}",
                cancellationToken);
        }
    }
}