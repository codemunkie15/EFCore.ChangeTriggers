using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Fixtures;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByEntity.Fixtures
{
    public class MigrateDatabaseUsingMigrationChangedByFixture : ChangedByEntityFixtureBase
    {
        public override bool MigrateDatabase => false;

        public MigrateDatabaseUsingMigrationChangedByFixture(MsSqlContainerFixture msSqlContainerFixture) : base(msSqlContainerFixture)
        {
        }
    }
}