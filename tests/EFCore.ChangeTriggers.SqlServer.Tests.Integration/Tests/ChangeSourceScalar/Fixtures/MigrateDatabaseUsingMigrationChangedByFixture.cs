using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Fixtures;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceScalar.Fixtures
{
    public class MigrateDatabaseUsingMigrationChangedByFixture : ChangeSourceScalarFixtureBase
    {
        public override bool MigrateDatabase => false;

        public MigrateDatabaseUsingMigrationChangedByFixture(MsSqlContainerFixture msSqlContainerFixture) : base(msSqlContainerFixture)
        {
        }
    }
}