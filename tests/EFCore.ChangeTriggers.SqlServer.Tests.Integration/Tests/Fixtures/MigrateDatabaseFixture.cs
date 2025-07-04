using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Fixtures;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.Fixtures
{
    public class MigrateDatabaseFixture : TestFixtureBase
    {
        public override bool MigrateDatabase => false;

        public MigrateDatabaseFixture(MsSqlContainerFixture msSqlContainerFixture) : base(msSqlContainerFixture)
        {
        }
    }
}