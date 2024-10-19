using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Infrastructure;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Persistence;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity
{
    public class ChangedByEntityMigrationFixture : ContainerFixture<ChangedByEntityDbContext>
    {
        public override bool MigrateDatabase => false;

        protected override IServiceProvider BuildServiceProvider(string connectionString)
        {
            return ChangedByEntityServiceProviderBuilder.Build(connectionString);
        }
    }
}
