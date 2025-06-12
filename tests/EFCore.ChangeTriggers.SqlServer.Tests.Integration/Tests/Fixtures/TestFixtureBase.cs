using EFCore.ChangeTriggers.Tests.Integration.Common.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Scopes;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.Fixtures
{
    public abstract class TestFixtureBase : DbContextFixture<TestDbContext>
    {
        public override string DatabaseNamePrefix => "Tests";

        protected TestFixtureBase(DbContainerFixture dbContainerFixture) : base(dbContainerFixture)
        {
        }

        public TestScope CreateTestScope()
        {
            return new(Services);
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddTestInfrastructure(GetConnectionString());
        }
    }
}