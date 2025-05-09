using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity
{
    public class ChangedByEntityFixture : ContainerFixture<ChangedByEntityDbContext>
    {
        public override bool MigrateDatabase => true;

        protected override IServiceProvider BuildServiceProvider(string connectionString)
        {
            return ChangedByEntityServiceProviderBuilder.Build(connectionString);
        }

        protected override void SetMigrationChangeContext()
        {
            var provider = Services.GetRequiredService<EntityCurrentUserProvider>();
            provider.CurrentUserAsync = new ChangedByEntityUser { Id = 0 };
        }
    }
}
