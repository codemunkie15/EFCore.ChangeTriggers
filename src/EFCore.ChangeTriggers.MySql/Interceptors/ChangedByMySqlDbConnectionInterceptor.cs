using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Constants;
using EFCore.ChangeTriggers.Infrastructure;
using EFCore.ChangeTriggers.Interceptors;
using EFCore.ChangeTriggers.MySql.Extensions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace EFCore.ChangeTriggers.MySql.Interceptors
{
    internal class ChangedByMySqlDbConnectionInterceptor<TChangedBy> : BaseChangedByDbConnectionInterceptor<TChangedBy>
    {
        public ChangedByMySqlDbConnectionInterceptor(
            ChangeTriggersExtensionContext changeTriggersExtensionContext,
            IChangedByProvider<TChangedBy> changedByProvider)
            : base(
                  changeTriggersExtensionContext,
                  changedByProvider)
        {
        }

        protected override void SetChangedByChangeContext(
            DbConnection connection,
            ConnectionEndEventData eventData,
            object? changedByProviderValue)
        {
            if (eventData.Connection.IsMasterDatabase())
            {
                // Database is probably being created, so don't set session context.
                return;
            }

            using var command = connection.CreateCommand();
            command.CommandText = $"SET {ChangeContextConstants.ChangedByContextName} = @changedBy;";
            var param = command.CreateParameter();
            param.ParameterName = "@changedBy";
            param.Value = changedByProviderValue;
            command.Parameters.Add(param);
            command.ExecuteNonQuery();
        }

        protected override async Task SetChangedByChangeContextAsync(
            DbConnection connection,
            ConnectionEndEventData eventData,
            object? changedByProviderValue,
            CancellationToken cancellationToken)
        {
            if (eventData.Connection.IsMasterDatabase())
            {
                // Database is probably being created, so don't set session context.
                return;
            }

            using var command = connection.CreateCommand();
            command.CommandText = $"SET {ChangeContextConstants.ChangedByContextName} = @changedBy;";
            var param = command.CreateParameter();
            param.ParameterName = "@changedBy";
            param.Value = changedByProviderValue;
            command.Parameters.Add(param);
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}
