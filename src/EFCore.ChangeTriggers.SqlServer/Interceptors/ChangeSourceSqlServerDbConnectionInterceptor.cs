using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Constants;
using EFCore.ChangeTriggers.Infrastructure;
using EFCore.ChangeTriggers.Interceptors;
using EFCore.ChangeTriggers.SqlServer.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Threading;

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

        protected override void SetChangeSourceChangeContext(DbConnection connection, object? changeSourceProviderValue)
        {
            if (connection.IsMasterDatabase())
            {
                // Database is probably being created, so don't set session context.
                return;
            }

            using var command = connection.CreateSetSessionContextCommand(ChangeContextConstants.ChangeSourceContextName, changeSourceProviderValue);
            command.ExecuteNonQuery();
        }

        protected override async Task SetChangeSourceChangeContextAsync(
            DbConnection connection,
            object? changeSourceProviderValue,
            CancellationToken cancellationToken)
        {
            if (connection.IsMasterDatabase())
            {
                // Database is probably being created, so don't set session context.
                return;
            }

            using var command = connection.CreateSetSessionContextCommand(ChangeContextConstants.ChangeSourceContextName, changeSourceProviderValue);
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}