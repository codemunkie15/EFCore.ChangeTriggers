using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceEntity.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceEntity.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity
{
    public class ChangeSourceEntityFixture : TestFixture<ChangeSourceEntityDbContext>
    {
        public override string DatabaseName => "ChangeSourceEntity";

        public override bool MigrateDatabase => true;

        public ChangeSourceEntityFixture(ContainerFixture sharedContainerFixture) : base(sharedContainerFixture)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddChangeSourceEntity(GetConnectionString());
        }

        protected override void SetMigrationChangeContext()
        {
            var provider = Services.GetRequiredService<EntityChangeSourceProvider>();
            provider.CurrentChangeSourceAsync = new ChangeSource { Id = 0 };
        }
    }
}
