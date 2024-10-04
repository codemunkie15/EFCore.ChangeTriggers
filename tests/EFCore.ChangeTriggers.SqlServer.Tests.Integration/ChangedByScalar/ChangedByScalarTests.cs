using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Domain;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Infrastructure;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar;

public class ChangedByScalarTests : IClassFixture<ChangedByScalarFixture>, IAsyncLifetime
{
    private readonly ChangedByScalarFixture fixture;

    public ChangedByScalarTests(ChangedByScalarFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public void MultipleScopes_SetsCorrectChangedBy()
    {
        const int numberOfUsers = 50;
        CreateUsers(numberOfUsers);

        var dbContext = fixture.Services.GetRequiredService<ChangedByScalarDbContext>();
        var testUsers = dbContext.TestUsers.Include(u => u.Changes).ToList();

        Assert.Equal(numberOfUsers, testUsers.Count);
        Assert.True(testUsers.All(u => u.Changes.All(c => c.ChangedBy == u.Username)));
    }

    [Fact]
    public async Task MultipleScopes_SetsCorrectChangedBy_Async()
    {
        const int numberOfUsers = 50;
        await CreateUsersAsync(numberOfUsers);

        var dbContext = fixture.Services.GetRequiredService<ChangedByScalarDbContext>();
        var testUsers = await dbContext.TestUsers.Include(u => u.Changes).ToListAsync();

        var instance = ServiceProviderCache.Instance;

        Assert.Equal(numberOfUsers, testUsers.Count);
        Assert.True(testUsers.All(u => u.Changes.All(c => c.ChangedBy == u.Username)));
    }

    private void CreateUsers(int numberOfUsers)
    {
        for (int i = 1; i <= numberOfUsers; i++)
        {
            using var scope = fixture.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ChangedByScalarDbContext>();
            var currentUserProvider = scope.ServiceProvider.GetRequiredService<ScalarCurrentUserProvider>();

            var username = $"TestUserScalar{i}";
            currentUserProvider.CurrentUser = username;

            var user = new ChangedByScalarUser
            {
                Username = username
            };

            dbContext.TestUsers.Add(user);
            dbContext.SaveChanges();
        }
    }

    private async Task CreateUsersAsync(int numberOfUsers)
    {
        for (int i = 1; i <= numberOfUsers; i++)
        {
            using var scope = fixture.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ChangedByScalarDbContext>();
            var currentUserProvider = scope.ServiceProvider.GetRequiredService<ScalarCurrentUserProvider>();

            var username = $"TestUserScalarAsync{i}";
            currentUserProvider.CurrentUser = username;

            var user = new ChangedByScalarUser
            {
                Username = username
            };

            dbContext.TestUsers.Add(user);
            await dbContext.SaveChangesAsync();
        }
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        var dbContext = fixture.Services.GetRequiredService<ChangedByScalarDbContext>();
        await dbContext.TestUsers.ExecuteDeleteAsync();
        await dbContext.TestUserChanges.ExecuteDeleteAsync();
    }
}