using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Scopes;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByEntity.Fixtures
{
    public class ChangedByEntityMigrationFixture : DbContextFixture<ChangedByEntityDbContext>
    {
        public override string DatabaseName => "ChangedByEntityMigration";

        public override bool MigrateDatabase => false;

        public ChangedByEntityMigrationFixture(MsSqlContainerFixture msSqlContainerFixture) : base(msSqlContainerFixture)
        {
        }

        public ChangedByEntityTestScope CreateTestScope()
        {
            return new ChangedByEntityTestScope(Services);
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddChangedByEntity(GetConnectionString());
        }
    }
}
