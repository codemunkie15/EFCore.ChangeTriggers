using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration
{
    public class ChangedByTests : IAsyncLifetime
    {
        private MsSqlContainer msSqlContainer;

        public Task InitializeAsync()
        {
            msSqlContainer = new MsSqlBuilder().Build();

            return msSqlContainer.StartAsync();
        }

        public Task DisposeAsync()
        {
            return msSqlContainer.DisposeAsync().AsTask();
        }

        [Fact]
        public void MultipleCalls_SetsCorrectChangedBy()
        {
            var services = new ServiceCollection();

            services.AddDbContext<TestDbContext>(options =>
            {
                options
                    .UseSqlServer("Data Source=localhost\\SQLEXPRESS;Database=ChangeTriggers;Integrated Security=True;Encrypt=False;TrustServerCertificate=False")
                    .UseSqlServerChangeTriggers<TestChangedByProvider, TestUser>();
            });
            services.AddScoped(services => new TestCurrentUserProvider(new TestUser { Id = 1 }));

            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TestDbContext>();

            var user = new TestUser
            {
                Username = "TestUser"
            };

            dbContext.TestUsers.Add(user);
            dbContext.SaveChanges();

            var userChange = dbContext.ChangeTracker.Entries<TestUser>().First().Entity.Changes.First();

            Assert.Equal(user.Id, userChange.ChangedBy.Id);
        }
    }
}