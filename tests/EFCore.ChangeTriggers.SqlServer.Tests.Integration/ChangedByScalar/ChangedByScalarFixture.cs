using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByScalar.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar
{
    public class ChangedByScalarFixture : ContainerFixture<ChangedByScalarDbContext>
    {
        public override bool MigrateDatabase => true;

        protected override IServiceProvider BuildServiceProvider(string connectionString)
        {
            return ChangedByScalarServiceProviderBuilder.Build(connectionString);
        }

        protected override void SetMigrationChangeContext()
        {
            var provider = Services.GetRequiredService<ScalarCurrentUserProvider>();
            provider.CurrentUserAsync = 0.ToString();
        }
    }
}
