using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Scopes;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceEntity.Fixtures
{
    public class ChangeSourceEntityMigrationFixture : DbContextFixture<ChangeSourceEntityDbContext>
    {
        public override string DatabaseName => "ChangeSourceEntityMigration";

        public override bool MigrateDatabase => false;

        public ChangeSourceEntityMigrationFixture(MsSqlContainerFixture msSqlContainerFixture) : base(msSqlContainerFixture)
        {
        }

        public ChangeSourceEntityTestScope CreateTestScope()
        {
            return new ChangeSourceEntityTestScope(Services);
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddChangeSourceEntity(GetConnectionString());
        }
    }
}
