using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangeSourceEntity;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceEntity.Fixtures
{
    public class ChangeSourceEntityFixture : DbContextFixture<ChangeSourceEntityDbContext>
    {
        public override string DatabaseName => "ChangeSourceEntity";

        public override bool MigrateDatabase => true;

        public ChangeSourceEntityFixture(MsSqlContainerFixture msSqlContainerFixture) : base(msSqlContainerFixture)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddChangeSourceEntity(GetConnectionString());
        }

        protected override void SetMigrationChangeContext()
        {
            var provider = Services.GetRequiredService<EntityChangeSourceProvider>();
            provider.CurrentChangeSourceAsync = new ChangeSource { Id = 1 };
        }
    }
}
