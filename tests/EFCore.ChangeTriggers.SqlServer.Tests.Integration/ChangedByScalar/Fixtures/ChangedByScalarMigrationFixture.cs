using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Configuration;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByScalar.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Fixtures
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
