using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Fixtures;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByEntity.Fixtures
{
    public class MigrateDatabaseFixture : ChangedByEntityFixtureBase
    {
        public override bool MigrateDatabase => false;

        public MigrateDatabaseFixture(MsSqlContainerFixture msSqlContainerFixture) : base(msSqlContainerFixture)
        {
        }
    }
}