using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Fixtures;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByScalar.Fixtures
{
    public class ScriptMigrationFixture : ChangedByScalarFixtureBase
    {
        public override bool MigrateDatabase => false;

        public ScriptMigrationFixture(MsSqlContainerFixture msSqlContainerFixture) : base(msSqlContainerFixture)
        {
        }
    }
}