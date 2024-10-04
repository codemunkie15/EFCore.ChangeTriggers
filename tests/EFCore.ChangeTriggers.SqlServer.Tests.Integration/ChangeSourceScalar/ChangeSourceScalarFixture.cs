using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Infrastructure;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar
{
    public class ChangeSourceScalarFixture : IAsyncLifetime
    {
        public IServiceProvider Services { get; private set; }

        private MsSqlContainer msSqlContainer;

        public async Task InitializeAsync()
        {
            msSqlContainer = new MsSqlBuilder().Build();

            await msSqlContainer.StartAsync();

            Services = ChangeSourceScalarServiceProviderBuilder.Build(msSqlContainer.GetConnectionString());
            var dbContext = Services.GetRequiredService<ChangeSourceScalarDbContext>();
            await dbContext.Database.MigrateAsync();
        }

        public async Task DisposeAsync()
        {
            await msSqlContainer.DisposeAsync().AsTask();
        }
    }
}
