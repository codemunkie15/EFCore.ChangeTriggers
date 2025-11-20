using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Constants;
using EFCore.ChangeTriggers.Infrastructure;
using EFCore.ChangeTriggers.Interceptors;
using EFCore.ChangeTriggers.SqlServer.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

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

        protected override void SetChangedByChangeContext(DbConnection connection, object? changedByProviderValue)
        {
            if (connection.IsMasterDatabase())
            {
                // Database is probably being created, so don't set session context.
                return;
            }

            using var command = connection.CreateSetSessionContextCommand(ChangeContextConstants.ChangedByContextName, changedByProviderValue);
            command.ExecuteNonQuery();
        }

        protected override async Task SetChangedByChangeContextAsync(
            DbConnection connection,
            object? changedByProviderValue,
            CancellationToken cancellationToken)
        {
            if (connection.IsMasterDatabase())
            {
                // Database is probably being created, so don't set session context.
                return;
            }

            using var command = connection.CreateSetSessionContextCommand(ChangeContextConstants.ChangedByContextName, changedByProviderValue);
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}
