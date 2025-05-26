using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Configuration;
using EFCore.ChangeTriggers.Tests.Integration.Common.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Fixtures
{
    public class ChangeSourceEntityMigrationFixture : DbContextFixture<ChangeSourceEntityDbContext>
    {
        public override string DatabaseName => "ChangeSourceEntityMigration";

        public override bool MigrateDatabase => false;

        public ChangeSourceEntityMigrationFixture(MsSqlContainerFixture msSqlContainerFixture) : base(msSqlContainerFixture)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddChangeSourceEntity(GetConnectionString());
        }
    }
}
