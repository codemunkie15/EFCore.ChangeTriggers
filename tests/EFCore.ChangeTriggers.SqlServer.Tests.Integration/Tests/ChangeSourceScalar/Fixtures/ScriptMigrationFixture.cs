using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Fixtures;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceScalar.Fixtures
{
    public class ScriptMigrationFixture : ChangeSourceScalarFixtureBase
    {
        public override bool MigrateDatabase => false;

        public ScriptMigrationFixture(MsSqlContainerFixture msSqlContainerFixture) : base(msSqlContainerFixture)
        {
        }
    }
}