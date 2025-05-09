using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceEntity.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceEntity.Infrastructure;
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
