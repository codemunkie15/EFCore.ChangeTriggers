using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangeSourceScalar;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceScalar.Fixtures
{
    public class ChangeSourceScalarFixture : DbContextFixture<ChangeSourceScalarDbContext>
    {
        public override string DatabaseName => "ChangeSourceScalar";

        public override bool MigrateDatabase => true;

        public ChangeSourceScalarFixture(MsSqlContainerFixture msSqlContainerFixture) : base(msSqlContainerFixture)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddChangeSourceScalar(GetConnectionString());
        }

        protected override void SetMigrationChangeContext()
        {
            var provider = Services.GetRequiredService<ScalarChangeSourceProvider>();
            provider.CurrentChangeSourceAsync = ChangeSourceType.Migrations;
        }
    }
}
