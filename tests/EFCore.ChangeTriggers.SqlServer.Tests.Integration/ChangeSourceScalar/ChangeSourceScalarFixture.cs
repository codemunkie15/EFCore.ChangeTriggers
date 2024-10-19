using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Domain;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Infrastructure;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar
{
    public class ChangeSourceScalarFixture : ContainerFixture<ChangeSourceScalarDbContext>
    {
        public override bool MigrateDatabase => true;

        protected override IServiceProvider BuildServiceProvider(string connectionString)
        {
            return ChangeSourceScalarServiceProviderBuilder.Build(connectionString);
        }

        protected override void SetMigrationChangeContext()
        {
            var provider = Services.GetRequiredService<ScalarChangeSourceProvider>();
            provider.CurrentChangeSourceAsync = ChangeSource.Migrations;
        }
    }
}
