using System.Data.Common;

namespace EFCore.ChangeTriggers.SqlServer.Extensions
{
    internal static class DbConnectionExtensions
    {
        public static bool IsMasterDatabase(this DbConnection connection)
        {
            return connection.Database == "master";
        }
    }
}