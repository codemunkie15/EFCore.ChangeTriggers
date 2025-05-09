using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar
{
    public class ChangedByScalarMigrationFixture : TestFixture<ChangedByScalarDbContext>
    {
        public override string DatabaseName => "ChangedByScalarMigration";

        public override bool MigrateDatabase => false;

        public ChangedByScalarMigrationFixture(ContainerFixture sharedContainerFixture) : base(sharedContainerFixture)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddChangedByScalar(GetConnectionString());
        }
    }
}
