using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Fixtures;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceEntity.Fixtures
{
    public class MigrateDatabaseUsingMigrationChangedByFixture : ChangeSourceEntityFixtureBase
    {
        public override bool MigrateDatabase => false;

        public MigrateDatabaseUsingMigrationChangedByFixture(MsSqlContainerFixture msSqlContainerFixture) : base(msSqlContainerFixture)
        {
        }
    }
}