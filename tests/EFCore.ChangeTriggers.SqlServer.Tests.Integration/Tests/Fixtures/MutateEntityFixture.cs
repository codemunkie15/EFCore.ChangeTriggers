using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Fixtures;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.Fixtures
{
    public class MutateEntityFixture : TestFixtureBase
    {
        public override bool MigrateDatabase => true;

        public MutateEntityFixture(MsSqlContainerFixture msSqlContainerFixture) : base(msSqlContainerFixture)
        {
        }
    }
}