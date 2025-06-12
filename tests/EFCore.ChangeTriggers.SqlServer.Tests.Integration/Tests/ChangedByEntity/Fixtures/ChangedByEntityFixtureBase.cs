using EFCore.ChangeTriggers.Tests.Integration.Common.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Scopes;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByEntity.Fixtures
{
    public abstract class ChangedByEntityFixtureBase : DbContextFixture<ChangedByEntityDbContext>
    {
        public override string DatabaseNamePrefix => "ChangedByEntity";

        protected ChangedByEntityFixtureBase(DbContainerFixture dbContainerFixture) : base(dbContainerFixture)
        {
        }

        public ChangedByEntityTestScope CreateTestScope()
        {
            return new ChangedByEntityTestScope(Services);
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddChangedByEntity(GetConnectionString());
        }
    }
}
