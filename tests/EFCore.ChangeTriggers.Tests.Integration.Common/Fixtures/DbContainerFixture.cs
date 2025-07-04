using DotNet.Testcontainers.Containers;
using Xunit;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Fixtures
{
    public abstract class DbContainerFixture
    {
        public abstract string GetConnectionString();
    }

    public abstract class DbContainerFixture<TDatabaseContainer> : DbContainerFixture, IAsyncLifetime
        where TDatabaseContainer : IDatabaseContainer
    {
        public TDatabaseContainer DbContainer { get; private set; }

        public virtual async ValueTask InitializeAsync()
        {
            DbContainer = BuildDbContainer();

            await DbContainer.StartAsync();
        }

        public virtual async ValueTask DisposeAsync()
        {
            await DbContainer.DisposeAsync().AsTask();
        }

        protected abstract TDatabaseContainer BuildDbContainer();
    }
}