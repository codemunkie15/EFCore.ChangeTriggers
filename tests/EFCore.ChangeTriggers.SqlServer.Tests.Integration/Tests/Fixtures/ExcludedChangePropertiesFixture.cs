using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.Fixtures
{
    public class ExcludedChangePropertiesFixture : DbContextFixture<ExcludedChangePropertiesDbContext>
    {

        public override string DatabaseNamePrefix => "ExcludedChangeProperties";

        public override bool MigrateDatabase => true;

        public ExcludedChangePropertiesFixture(MsSqlContainerFixture msSqlContainerFixture) : base(msSqlContainerFixture)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddExcludedChangePropertiesServices(GetConnectionString());
        }
    }
}
