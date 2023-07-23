using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using EntityFrameworkCore.ChangeTrackingTriggers.Constants;
using EntityFrameworkCore.ChangeTrackingTriggers.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ChangeTrackingTriggers.SqlServer.Interceptors
{
    internal class ChangeSourceDbConnectionInterceptor<TChangeSource> : BaseChangeSourceDbConnectionInterceptor<TChangeSource>
    {
        public ChangeSourceDbConnectionInterceptor(
            IChangeSourceProvider<TChangeSource> changeSourceTypeProvider)
            : base(changeSourceTypeProvider)
        {
        }

        protected override async Task SetChangeSourceChangeTrackingContextAsync(
            ConnectionEndEventData eventData,
            object? changeSourceRawValue,
            CancellationToken cancellationToken)
        {
            var changeSourceProviderValue = eventData.Context?.ConvertToProvider<TChangeSource>(changeSourceRawValue);

            await eventData.Context!.Database.ExecuteSqlAsync(
                $"EXEC sp_set_session_context {ChangeTrackingContextConstants.ChangeSourceContextName}, {changeSourceProviderValue}",
                cancellationToken);
        }
    }
}