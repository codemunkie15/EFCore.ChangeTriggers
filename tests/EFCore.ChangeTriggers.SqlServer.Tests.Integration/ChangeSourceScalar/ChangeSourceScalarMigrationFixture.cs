using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Persistence;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar
{
    public class ChangeSourceScalarMigrationFixture : ContainerFixture<ChangeSourceScalarDbContext>
    {
        public override bool MigrateDatabase => false;

        protected override IServiceProvider BuildServiceProvider(string connectionString)
        {
            return ChangeSourceScalarServiceProviderBuilder.Build(connectionString);
        }
    }
}
