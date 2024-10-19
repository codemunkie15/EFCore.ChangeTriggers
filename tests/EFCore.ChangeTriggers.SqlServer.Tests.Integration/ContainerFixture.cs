using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration
{
    public abstract class ContainerFixture<TDbContext> : IAsyncLifetime
        where TDbContext : DbContext
    {
        public IServiceProvider Services { get; private set; }

        public abstract bool MigrateDatabase { get; }

        protected MsSqlContainer msSqlContainer;

        public virtual async Task InitializeAsync()
        {
            msSqlContainer = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .Build();

            await msSqlContainer.StartAsync();

            Services = BuildServiceProvider(msSqlContainer.GetConnectionString());

            if (MigrateDatabase)
            {
                SetMigrationChangeContext();
                var dbContext = Services.GetRequiredService<TDbContext>();
                await dbContext.Database.MigrateAsync();
            }
        }

        public virtual async Task DisposeAsync()
        {
            await msSqlContainer.DisposeAsync().AsTask();
        }

        protected abstract IServiceProvider BuildServiceProvider(string connectionString);

        protected virtual void SetMigrationChangeContext()
        {
        }
    }
}
