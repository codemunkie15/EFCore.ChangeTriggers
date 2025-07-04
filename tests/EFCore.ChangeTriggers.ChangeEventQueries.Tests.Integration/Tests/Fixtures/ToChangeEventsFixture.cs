using EFCore.ChangeTriggers.Tests.Integration.Common.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.Tests.Fixtures
{
    public class ToChangeEventsFixture : DbContextFixture<TestDbContext>
    {
        public override string DatabaseNamePrefix => "Tests";

        public override bool MigrateDatabase => true;

        public ToChangeEventsFixture(MsSqlContainerFixture msSqlContainerFixture) : base(msSqlContainerFixture)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddTestInfrastructure(GetConnectionString());
        }
    }
}
