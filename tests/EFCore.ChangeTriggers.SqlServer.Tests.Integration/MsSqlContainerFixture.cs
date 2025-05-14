using EFCore.ChangeTriggers.Tests.Integration.Common.Fixtures;
using Testcontainers.MsSql;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration
{
    [CollectionDefinition("MsSqlContainer")]
    public class MsSqlContainerCollection : ICollectionFixture<MsSqlContainerFixture>
    {
    }

    public class MsSqlContainerFixture : DbContainerFixture<MsSqlContainer>
    {
        protected override MsSqlContainer BuildDbContainer()
        {
            return new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .Build();
        }

        public override string GetConnectionString()
        {
            return DbContainer.GetConnectionString();
        }
    }
}