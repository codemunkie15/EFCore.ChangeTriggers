using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Fixtures;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceEntity.Fixtures
{
    public class ScriptMigrationFixture : ChangeSourceEntityFixtureBase
    {
        public override bool MigrateDatabase => false;

        public ScriptMigrationFixture(MsSqlContainerFixture msSqlContainerFixture) : base(msSqlContainerFixture)
        {
        }
    }
}