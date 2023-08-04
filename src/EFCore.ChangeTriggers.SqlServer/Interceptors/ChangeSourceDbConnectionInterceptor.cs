using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Constants;
using EFCore.ChangeTriggers.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EFCore.ChangeTriggers.SqlServer.Interceptors
{
    internal class ChangeSourceDbConnectionInterceptor<TChangeSource> : BaseChangeSourceDbConnectionInterceptor<TChangeSource>
    {
        public ChangeSourceDbConnectionInterceptor(
            IChangeSourceProvider<TChangeSource> changeSourceProvider)
            : base(changeSourceProvider)
        {
        }

        protected override async Task SetChangeSourceChangeContextAsync(
            ConnectionEndEventData eventData,
            object? changeSourceRawValue,
            CancellationToken cancellationToken)
        {
            var changeSourceProviderValue = eventData.Context?.ConvertToProvider<TChangeSource>(changeSourceRawValue);

            await eventData.Context!.Database.ExecuteSqlAsync(
                $"EXEC sp_set_session_context {ChangeContextConstants.ChangeSourceContextName}, {changeSourceProviderValue}",
                cancellationToken);
        }
    }
}