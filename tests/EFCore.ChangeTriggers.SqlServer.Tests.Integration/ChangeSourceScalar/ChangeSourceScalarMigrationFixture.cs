using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar
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
