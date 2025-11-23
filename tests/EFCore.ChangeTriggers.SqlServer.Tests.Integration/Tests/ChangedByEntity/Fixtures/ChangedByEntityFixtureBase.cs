using EFCore.ChangeTriggers.Tests.Integration.Common.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByEntity.Fixtures
{
    public abstract class ChangedByEntityFixtureBase : DbContextFixture<ChangedByEntityDbContext>
    {
        public override string DatabaseNamePrefix => "ChangedByEntity";

        protected ChangedByEntityFixtureBase(DbContainerFixture dbContainerFixture) : base(dbContainerFixture)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddChangedByEntityServices(GetConnectionString());
        }
    }
}
