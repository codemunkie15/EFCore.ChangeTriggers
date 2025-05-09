using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity
{
    public class ChangedByEntityMigrationFixture : TestFixture<ChangedByEntityDbContext>
    {
        public override string DatabaseName => "ChangedByEntityMigration";

        public override bool MigrateDatabase => false;

        public ChangedByEntityMigrationFixture(ContainerFixture sharedContainerFixture) : base(sharedContainerFixture)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddChangedByEntity(GetConnectionString());
        }
    }
}
