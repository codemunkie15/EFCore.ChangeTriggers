using DotNet.Testcontainers.Builders;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Infrastructure;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity
{
    public class ChangedByEntityFixture : IAsyncLifetime
    {
        public IServiceProvider Services { get; private set; }

        private MsSqlContainer msSqlContainer;

        public async Task InitializeAsync()
        {
            msSqlContainer = TestContainerBuilder.MsSql().Build();

            await msSqlContainer.StartAsync();

            Services = ChangedByEntityServiceProviderBuilder.Build(msSqlContainer.GetConnectionString());
            
            var dbContext = Services.GetRequiredService<ChangedByEntityDbContext>();

            await dbContext.Database.MigrateAsync();
        }

        public async Task DisposeAsync()
        {
            await msSqlContainer.DisposeAsync().AsTask();
        }
    }
}
