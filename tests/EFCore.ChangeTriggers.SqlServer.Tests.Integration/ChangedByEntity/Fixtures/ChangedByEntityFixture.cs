using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Configuration;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Infrastructure;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Fixtures
{
    public class ChangedByEntityFixture : DbContextFixture<ChangedByEntityDbContext>
    {
        public override string DatabaseName => "ChangedByEntity";

        public override bool MigrateDatabase => true;

        public ChangedByEntityFixture(MsSqlContainerFixture msSqlContainerFixture) : base(msSqlContainerFixture)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddChangedByEntity(GetConnectionString());
        }

        protected override void SetMigrationChangeContext()
        {
            var provider = Services.GetRequiredService<EntityCurrentUserProvider>();
            provider.CurrentUserAsync = ChangedByEntityUser.SystemUser;
        }
    }
}
