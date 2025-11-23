using EFCore.ChangeTriggers.Tests.Integration.Common.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByScalar.Fixtures
{
    public abstract class ChangedByScalarFixtureBase : DbContextFixture<ChangedByScalarDbContext>
    {
        public override string DatabaseNamePrefix => "ChangedByScalar";

        protected ChangedByScalarFixtureBase(DbContainerFixture dbContainerFixture) : base(dbContainerFixture)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddChangedByScalarServices(GetConnectionString());
        }
    }
}
