using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Configuration;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByScalar;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Fixtures
{
    public class ChangedByScalarFixture : DbContextFixture<ChangedByScalarDbContext>
    {
        public override string DatabaseName => "ChangedByScalar";

        public override bool MigrateDatabase => true;

        public ChangedByScalarFixture(MsSqlContainerFixture msSqlContainerFixture) : base(msSqlContainerFixture)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddChangedByScalar(GetConnectionString());
        }

        protected override void SetMigrationChangeContext()
        {
            var provider = Services.GetRequiredService<ScalarCurrentUserProvider>();
            provider.CurrentUserAsync = ChangedByScalarUser.SystemUser.Username;
        }
    }
}
