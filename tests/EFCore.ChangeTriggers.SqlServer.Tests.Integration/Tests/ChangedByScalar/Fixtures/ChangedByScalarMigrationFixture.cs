using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByScalar.Fixtures
{
    public class ChangedByScalarMigrationFixture : DbContextFixture<ChangedByScalarDbContext>
    {
        public override string DatabaseName => "ChangedByScalarMigration";

        public override bool MigrateDatabase => false;

        public ChangedByScalarMigrationFixture(MsSqlContainerFixture msSqlContainerFixture) : base(msSqlContainerFixture)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddChangedByScalar(GetConnectionString());
        }
    }
}
