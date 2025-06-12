using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByScalar;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByScalar.Fixtures
{
    public class MutateEntityFixture : ChangedByScalarFixtureBase
    {
        public override bool MigrateDatabase => true;

        public MutateEntityFixture(MsSqlContainerFixture msSqlContainerFixture) : base(msSqlContainerFixture)
        {
        }

        protected override void SetMigrationChangeContext()
        {
            var provider = Services.GetRequiredService<ScalarCurrentUserProvider>();
            provider.CurrentUser.AsyncValue = ChangedByScalarUser.SystemUser.Username;
        }
    }
}