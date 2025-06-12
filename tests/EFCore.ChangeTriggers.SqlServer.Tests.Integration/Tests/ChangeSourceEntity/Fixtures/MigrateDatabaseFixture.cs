using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Fixtures;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceEntity.Fixtures
{
    public class MigrateDatabaseFixture : ChangeSourceEntityFixtureBase
    {
        public override bool MigrateDatabase => false;

        public MigrateDatabaseFixture(MsSqlContainerFixture msSqlContainerFixture) : base(msSqlContainerFixture)
        {
        }
    }
}