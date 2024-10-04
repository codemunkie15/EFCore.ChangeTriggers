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
        CreateUsers();

        var dbContext = fixture.Services.GetRequiredService<ChangedByScalarDbContext>();
        var users = dbContext.TestUsers.Include(u => u.Changes).ToList();

        Assert.True(users.All(u => u.Changes.All(c => c.ChangedBy == u.Username)));
    }

    [Fact]
    public async Task MultipleScopes_SetsCorrectChangedBy_Async()
    {
        await CreateUsersAsync();

        var dbContext = fixture.Services.GetRequiredService<ChangedByScalarDbContext>();
        var users = await dbContext.TestUsers.Include(u => u.Changes).ToListAsync();

        var instance = ServiceProviderCache.Instance;

        Assert.True(users.All(u => u.Changes.All(c => c.ChangedBy == u.Username)));
    }

    private void CreateUsers()
    {
        for (int i = 1; i <= 50; i++)
        {
            using var scope = fixture.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ChangedByScalarDbContext>();
            var currentUserProvider = scope.ServiceProvider.GetRequiredService<ChangedByScalarCurrentUserProvider>();

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

    private async Task CreateUsersAsync()
    {
        for (int i = 51; i <= 100; i++)
        {
            using var scope = fixture.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ChangedByScalarDbContext>();
            var currentUserProvider = scope.ServiceProvider.GetRequiredService<ChangedByScalarCurrentUserProvider>();

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