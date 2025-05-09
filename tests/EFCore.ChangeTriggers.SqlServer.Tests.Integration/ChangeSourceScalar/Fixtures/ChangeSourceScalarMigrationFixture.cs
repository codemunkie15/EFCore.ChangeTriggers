using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Configuration;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceScalar.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Fixtures
{
    public class ChangeSourceScalarMigrationFixture : TestFixture<ChangeSourceScalarDbContext>
    {
        public override string DatabaseName => "ChangeSourceScalarMigration";

        public override bool MigrateDatabase => false;

        public ChangeSourceScalarMigrationFixture(ContainerFixture sharedContainerFixture) : base(sharedContainerFixture)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddChangeSourceScalar(GetConnectionString());
        }
    }
}
