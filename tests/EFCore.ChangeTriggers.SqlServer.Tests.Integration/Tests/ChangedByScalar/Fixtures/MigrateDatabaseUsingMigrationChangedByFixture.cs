using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Fixtures;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByScalar.Fixtures
{
    public class MigrateDatabaseUsingMigrationChangedByFixture : ChangedByScalarFixtureBase
    {
        public override bool MigrateDatabase => false;

        public MigrateDatabaseUsingMigrationChangedByFixture(MsSqlContainerFixture msSqlContainerFixture) : base(msSqlContainerFixture)
        {
        }
    }
}