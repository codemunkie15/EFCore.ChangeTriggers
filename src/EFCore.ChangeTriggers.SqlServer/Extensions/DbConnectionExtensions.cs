using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace EFCore.ChangeTriggers.SqlServer.Extensions
{
    internal static class DbConnectionExtensions
    {
        public static bool IsMasterDatabase(this DbConnection connection)
        {
            return connection.Database == "master";
        }

        public static DbCommand CreateSetSessionContextCommand(this DbConnection connection, string key, object? value)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = "EXEC sp_set_session_context @key, @value;";
            cmd.Parameters.Add(new SqlParameter("@key", key));
            cmd.Parameters.Add(new SqlParameter("@value", value ?? DBNull.Value));

            return cmd;
        }
    }
}