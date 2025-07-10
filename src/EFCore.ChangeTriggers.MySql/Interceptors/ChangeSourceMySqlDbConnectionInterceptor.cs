using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Constants;
using EFCore.ChangeTriggers.Infrastructure;
using EFCore.ChangeTriggers.Interceptors;
using EFCore.ChangeTriggers.MySql.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace EFCore.ChangeTriggers.MySql.Interceptors
{
    internal class ChangeSourceMySqlDbConnectionInterceptor<TChangeSource> : BaseChangeSourceDbConnectionInterceptor<TChangeSource>
    {
        public ChangeSourceMySqlDbConnectionInterceptor(
            ChangeTriggersExtensionContext changeTriggersExtensionContext,
            IChangeSourceProvider<TChangeSource> changeSourceProvider)
            : base(
                  changeTriggersExtensionContext,
                  changeSourceProvider)
        {
        }

        protected override void SetChangeSourceChangeContext(
            DbConnection connection,
            ConnectionEndEventData eventData,
            object? changeSourceProviderValue)
        {
            if (eventData.Connection.IsMasterDatabase())
            {
                // Database is probably being created, so don't set session context.
                return;
            }

            using var command = connection.CreateCommand();
            command.CommandText = $"SET {ChangeContextConstants.ChangeSourceContextName} = @changeSource;";
            var param = command.CreateParameter();
            param.ParameterName = "@changeSource";
            param.Value = changeSourceProviderValue;
            command.Parameters.Add(param);
            command.ExecuteNonQuery();
        }

        protected override async Task SetChangeSourceChangeContextAsync(
            DbConnection connection,
            ConnectionEndEventData eventData,
            object? changeSourceProviderValue,
            CancellationToken cancellationToken)
        {
            if (eventData.Connection.IsMasterDatabase())
            {
                // Database is probably being created, so don't set session context.
                return;
            }

            using var command = connection.CreateCommand();
            command.CommandText = $"SET {ChangeContextConstants.ChangeSourceContextName} = @changeSource;";
            var param = command.CreateParameter();
            param.ParameterName = "@changeSource";
            param.Value = changeSourceProviderValue;
            command.Parameters.Add(param);
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}