using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Configuration;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceScalar.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceScalar.Infrastructure;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceScalar.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Fixtures
{
    public class ChangeSourceScalarFixture : TestFixture<ChangeSourceScalarDbContext>
    {
        public override string DatabaseName => "ChangeSourceScalar";

        public override bool MigrateDatabase => true;

        public ChangeSourceScalarFixture(ContainerFixture sharedContainerFixture) : base(sharedContainerFixture)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddChangeSourceScalar(GetConnectionString());
        }

        protected override void SetMigrationChangeContext()
        {
            var provider = Services.GetRequiredService<ScalarChangeSourceProvider>();
            provider.CurrentChangeSourceAsync = ChangeSource.Migrations;
        }
    }
}
