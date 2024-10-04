using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Infrastructure;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar
{
    public class ChangedByScalarFixture : IAsyncLifetime
    {
        public IServiceProvider Services { get; private set; }

        private MsSqlContainer msSqlContainer;

        public async Task InitializeAsync()
        {
            msSqlContainer = new MsSqlBuilder().Build();

            await msSqlContainer.StartAsync();

            Services = ChangedByScalarServiceProviderBuilder.Build(msSqlContainer.GetConnectionString());
            var dbContext = Services.GetRequiredService<ChangedByScalarDbContext>();
            await dbContext.Database.MigrateAsync();
        }

        public async Task DisposeAsync()
        {
            await msSqlContainer.DisposeAsync().AsTask();
        }
    }
}
