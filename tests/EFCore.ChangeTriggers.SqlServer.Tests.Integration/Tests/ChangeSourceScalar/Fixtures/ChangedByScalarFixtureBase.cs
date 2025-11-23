using EFCore.ChangeTriggers.Tests.Integration.Common.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceScalar.Fixtures
{
    public abstract class ChangeSourceScalarFixtureBase : DbContextFixture<ChangeSourceScalarDbContext>
    {
        public override string DatabaseNamePrefix => "ChangeSourceScalar";

        protected ChangeSourceScalarFixtureBase(DbContainerFixture dbContainerFixture) : base(dbContainerFixture)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddChangeSourceScalarServices(GetConnectionString());
        }
    }
}
