using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Fixtures;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByEntity.Fixtures
{
    public class ScriptMigrationFixture : ChangedByEntityFixtureBase
    {
        public override bool MigrateDatabase => false;

        public ScriptMigrationFixture(MsSqlContainerFixture msSqlContainerFixture) : base(msSqlContainerFixture)
        {
        }
    }
}