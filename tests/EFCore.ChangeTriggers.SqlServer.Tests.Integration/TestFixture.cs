using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration
{
    public abstract class TestFixture<TDbContext> : IAsyncLifetime
        where TDbContext : DbContext
    {
        public ServiceProvider Services { get; private set; }

        public abstract string DatabaseName { get; }

        public abstract bool MigrateDatabase { get; }

        public ContainerFixture ContainerFixture { get; }

        protected TestFixture(ContainerFixture sharedContainerFixture)
        {
            ContainerFixture = sharedContainerFixture;
        }

        public virtual async Task InitializeAsync()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            Services = serviceCollection.BuildServiceProvider();

            if (MigrateDatabase)
            {
                SetMigrationChangeContext();
                var dbContext = Services.GetRequiredService<TDbContext>();
                await dbContext.Database.MigrateAsync();
            }
        }

        public virtual async Task DisposeAsync()
        {
            await Services.DisposeAsync();
        }

        protected abstract void ConfigureServices(IServiceCollection services);

        protected virtual void SetMigrationChangeContext() { }

        protected string GetConnectionString()
        {
            var builder = new SqlConnectionStringBuilder(ContainerFixture.MsSqlContainer.GetConnectionString())
            {
                InitialCatalog = DatabaseName
            };

            return builder.ToString();
        }
    }
}