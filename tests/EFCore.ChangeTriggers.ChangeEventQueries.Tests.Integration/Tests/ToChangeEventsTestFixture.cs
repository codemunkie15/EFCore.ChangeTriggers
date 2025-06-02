using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByEntity;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.Tests
{
    public class ToChangeEventsTestFixture : DbContextFixture<TestDbContext>
    {
        public override string DatabaseName => "ToChangeEvents";

        public override bool MigrateDatabase => true;

        public ToChangeEventsTestFixture(MsSqlContainerFixture msSqlContainerFixture) : base(msSqlContainerFixture)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddSqlServerChangeEventQueries<TestDbContext>(GetConnectionString());
        }
    }
}
