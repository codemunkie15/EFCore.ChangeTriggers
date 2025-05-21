using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Fixtures
{
    public abstract class DbContextFixture<TDbContext> : IAsyncLifetime
        where TDbContext : DbContext
    {
        public DbContainerFixture DbContainerFixture { get; }

        public ServiceProvider Services { get; private set; }

        public abstract string DatabaseName { get; }

        public abstract bool MigrateDatabase { get; }

        protected DbContextFixture(DbContainerFixture dbContainerFixture)
        {
            DbContainerFixture = dbContainerFixture;
        }

        public virtual async ValueTask InitializeAsync()
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

        public virtual async ValueTask DisposeAsync()
        {
            await Services.DisposeAsync();
        }

        protected abstract void ConfigureServices(IServiceCollection services);

        protected virtual void SetMigrationChangeContext() { }

        public string GetConnectionString()
        {
            var builder = new SqlConnectionStringBuilder(DbContainerFixture.GetConnectionString())
            {
                InitialCatalog = DatabaseName
            };

            return builder.ToString();
        }
    }
}