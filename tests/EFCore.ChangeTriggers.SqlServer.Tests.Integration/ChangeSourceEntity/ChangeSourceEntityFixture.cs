using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Domain;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Infrastructure;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity
{
    public class ChangeSourceEntityFixture : ContainerFixture<ChangeSourceEntityDbContext>
    {
        public override bool MigrateDatabase => true;

        protected override IServiceProvider BuildServiceProvider(string connectionString)
        {
            return ChangeSourceEntityServiceProviderBuilder.Build(connectionString);
        }

        protected override void SetMigrationChangeContext()
        {
            var provider = Services.GetRequiredService<EntityChangeSourceProvider>();
            provider.CurrentChangeSourceAsync = new ChangeSource { Id = 0 };
        }
    }
}
