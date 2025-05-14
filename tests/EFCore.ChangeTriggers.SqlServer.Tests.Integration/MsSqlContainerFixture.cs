using EFCore.ChangeTriggers.SqlServer.Tests.Integration;
using EFCore.ChangeTriggers.Tests.Integration.Common.Fixtures;
using Testcontainers.MsSql;

[assembly: AssemblyFixture(typeof(MsSqlContainerFixture))]

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration
{
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