using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Configuration;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Fixtures
{
    public class ChangedByEntityMigrationFixture : DbContextFixture<ChangedByEntityDbContext>
    {
        public override string DatabaseName => "ChangedByEntityMigration";

        public override bool MigrateDatabase => false;

        public ChangedByEntityMigrationFixture(MsSqlContainerFixture msSqlContainerFixture) : base(msSqlContainerFixture)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddChangedByEntity(GetConnectionString());
        }
    }
}
