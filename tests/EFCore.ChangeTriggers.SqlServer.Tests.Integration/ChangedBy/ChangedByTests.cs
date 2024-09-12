using EFCore.ChangeTriggers.SqlServer.Extensions;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedBy.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedBy;

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
    public async void MultipleScopes_SetsCorrectChangedBy()
    {
        var services = BuildServiceProvider();

        var dbContext = services.GetRequiredService<TestChangedByDbContext>();

        var migrations = dbContext.Database.GetPendingMigrations();
        await dbContext.Database.MigrateAsync();

        await CreateUsersAsync(services);

        var users = await dbContext.TestUsers.Include(u => u.Changes).ToListAsync();

        Assert.True(users.All(u => u.Changes.All(c => c.ChangedBy.Id == u.Id)));
    }

    private ServiceProvider BuildServiceProvider()
    {
        return new ServiceCollection()
            .AddDbContext<TestChangedByDbContext>(options =>
            {
                options
                    .UseSqlServer(msSqlContainer.GetConnectionString(), options =>
                    {
                        options.MigrationsAssembly("EFCore.ChangeTriggers.SqlServer.Tests.Integration.Migrations");
                    })
                    .UseSqlServerChangeTriggers(options =>
                    {
                        options.UseChangedBy<TestChangedByProvider, TestUser>();
                    });
            })
            .AddSingleton(services => new TestCurrentUserProvider(new TestUser { Id = 1 }))
            .BuildServiceProvider();
    }

    //private void ScaffoldMigration()
    //{
    //    var services = BuildServiceProvider();
    //    var dbContext = services.GetRequiredService<TestChangedByDbContext>();

    //    var designTimeServices = new DesignTimeServicesBuilder(
    //        typeof(TestChangedByDbContext).Assembly,
    //        typeof(TestChangedByDbContext).Assembly,
    //        new OperationReporter(null), [])
    //            .Build(dbContext);

    //    var migrationsScaffolder = designTimeServices.GetRequiredService<IMigrationsScaffolder>();
    //    var migration = migrationsScaffolder.ScaffoldMigration("Initial", GetType().Assembly.GetName().Name);
    //    var migrationsDirectory = Path.GetDirectoryName(TestChangedByDbContext.GetFilePath());
    //    var result = migrationsScaffolder.Save(migrationsDirectory, migration, null);
    //}

    private async Task CreateUsersAsync(IServiceProvider serviceProvider)
    {
        for (int i = 1; i <= 500; i++)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var currentUserProvider = scope.ServiceProvider.GetRequiredService<TestCurrentUserProvider>();
                currentUserProvider.CurrentUser = new TestUser { Id = i };

                var dbContext = scope.ServiceProvider.GetRequiredService<TestChangedByDbContext>();

                var user = new TestUser
                {
                    Username = $"TestUser{i}"
                };

                dbContext.TestUsers.Add(user);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}