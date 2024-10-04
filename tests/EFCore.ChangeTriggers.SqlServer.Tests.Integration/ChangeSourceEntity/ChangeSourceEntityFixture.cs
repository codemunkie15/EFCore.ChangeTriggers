using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Infrastructure;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity
{
    public class ChangeSourceEntityFixture : IAsyncLifetime
    {
        public IServiceProvider Services { get; private set; }

        private MsSqlContainer msSqlContainer;

        public async Task InitializeAsync()
        {
            msSqlContainer = new MsSqlBuilder().Build();

            await msSqlContainer.StartAsync();

            Services = ChangeSourceEntityServiceProviderBuilder.Build(msSqlContainer.GetConnectionString());
            var dbContext = Services.GetRequiredService<ChangeSourceEntityDbContext>();
            await dbContext.Database.MigrateAsync();
        }

        public async Task DisposeAsync()
        {
            await msSqlContainer.DisposeAsync().AsTask();
        }
    }
}
