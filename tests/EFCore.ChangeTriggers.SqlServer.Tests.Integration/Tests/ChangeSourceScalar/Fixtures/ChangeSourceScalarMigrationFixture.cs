using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceScalar.Fixtures
{
    public class ChangeSourceScalarMigrationFixture : DbContextFixture<ChangeSourceScalarDbContext>
    {
        public override string DatabaseName => "ChangeSourceScalarMigration";

        public override bool MigrateDatabase => false;

        public ChangeSourceScalarMigrationFixture(MsSqlContainerFixture msSqlContainerFixture) : base(msSqlContainerFixture)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddChangeSourceScalar(GetConnectionString());
        }
    }
}
