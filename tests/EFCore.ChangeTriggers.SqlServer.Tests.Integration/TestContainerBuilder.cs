using Testcontainers.MsSql;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration
{
    public static class TestContainerBuilder
    {
        public static MsSqlBuilder MsSql()
        {
            return new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest");
        }
    }
}