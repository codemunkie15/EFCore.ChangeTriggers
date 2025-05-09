using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Persistence;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity
{
    public class ChangeSourceEntityMigrationFixture : ContainerFixture<ChangeSourceEntityDbContext>
    {
        public override bool MigrateDatabase => false;

        protected override IServiceProvider BuildServiceProvider(string connectionString)
        {
            return ChangeSourceEntityServiceProviderBuilder.Build(connectionString);
        }
    }
}
