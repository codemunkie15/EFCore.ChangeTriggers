using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Persistence;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar
{
    public class ChangedByScalarMigrationFixture : ContainerFixture<ChangedByScalarDbContext>
    {
        public override bool MigrateDatabase => false;

        protected override IServiceProvider BuildServiceProvider(string connectionString)
        {
            return ChangedByScalarServiceProviderBuilder.Build(connectionString);
        }
    }
}
