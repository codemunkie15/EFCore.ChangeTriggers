using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Configuration;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceEntity.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Fixtures
{
    public class ChangeSourceEntityMigrationFixture : TestFixture<ChangeSourceEntityDbContext>
    {
        public override string DatabaseName => "ChangeSourceEntityMigration";

        public override bool MigrateDatabase => false;

        public ChangeSourceEntityMigrationFixture(ContainerFixture sharedContainerFixture) : base(sharedContainerFixture)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddChangeSourceEntity(GetConnectionString());
        }
    }
}
