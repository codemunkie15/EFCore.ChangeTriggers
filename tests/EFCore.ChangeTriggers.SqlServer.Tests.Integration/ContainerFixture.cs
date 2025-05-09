using Testcontainers.MsSql;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration
{
    [CollectionDefinition("SharedContainer")]
    public class ContainerCollection : ICollectionFixture<ContainerFixture>
    {
    }
    public class ContainerFixture : IAsyncLifetime
    {
        public MsSqlContainer MsSqlContainer { get; private set; }

        public virtual async Task InitializeAsync()
        {
            MsSqlContainer = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .Build();

            await MsSqlContainer.StartAsync();
        }

        public virtual async Task DisposeAsync()
        {
            await MsSqlContainer.DisposeAsync().AsTask();
        }
    }
}