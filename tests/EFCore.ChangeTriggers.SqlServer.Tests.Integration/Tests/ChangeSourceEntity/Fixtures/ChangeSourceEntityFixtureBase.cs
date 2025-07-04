using EFCore.ChangeTriggers.Tests.Integration.Common.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Scopes;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceEntity.Fixtures
{
    public abstract class ChangeSourceEntityFixtureBase : DbContextFixture<ChangeSourceEntityDbContext>
    {
        public override string DatabaseNamePrefix => "ChangeSourceEntity";

        protected ChangeSourceEntityFixtureBase(DbContainerFixture dbContainerFixture) : base(dbContainerFixture)
        {
        }

        public ChangeSourceEntityTestScope CreateTestScope()
        {
            return new ChangeSourceEntityTestScope(Services);
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddChangeSourceEntity(GetConnectionString());
        }
    }
}
