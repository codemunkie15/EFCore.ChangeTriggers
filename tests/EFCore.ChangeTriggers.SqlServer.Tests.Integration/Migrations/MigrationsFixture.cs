using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Migrations.Infrastructure;
using Testcontainers.MsSql;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Migrations
{
    public class MigrationsFixture : IAsyncLifetime
    {
        public IServiceProvider Services { get; private set; }

        private MsSqlContainer msSqlContainer;

        public async Task InitializeAsync()
        {
            msSqlContainer = new MsSqlBuilder().Build();

            await msSqlContainer.StartAsync();

            Services = MigrationsServiceProviderBuilder.Build(msSqlContainer.GetConnectionString());
        }

        public async Task DisposeAsync()
        {
            await msSqlContainer.DisposeAsync().AsTask();
        }
    }
}
