using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Infrastructure;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity;

public class ChangedByEntityTests : IClassFixture<ChangedByEntityFixture>, IAsyncLifetime
{
    private readonly ChangedByEntityFixture fixture;

    public ChangedByEntityTests(ChangedByEntityFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public void MultipleScopes_SetsCorrectChangedBy()
    {
        CreateUsers();

        var dbContext = fixture.Services.GetRequiredService<ChangedByEntityDbContext>();
        var users = dbContext.TestUsers.Include(u => u.Changes).ThenInclude(c => c.ChangedBy).ToList();

        Assert.True(users.All(u => u.Changes.All(c => c.ChangedBy.Id == u.Id)));
    }

    [Fact]
    public async Task MultipleScopes_SetsCorrectChangedBy_Async()
    {
        await CreateUsersAsync();

        var dbContext = fixture.Services.GetRequiredService<ChangedByEntityDbContext>();
        var users = await dbContext.TestUsers.Include(u => u.Changes).ToListAsync();

        Assert.True(users.All(u => u.Changes.All(c => c.ChangedBy.Id == u.Id)));
    }

    private void CreateUsers()
    {
        for (int i = 1; i <= 50; i++)
        {
            using var scope = fixture.Services.CreateScope();
            var currentUserProvider = scope.ServiceProvider.GetRequiredService<ChangedByEntityCurrentUserProvider>();
            currentUserProvider.CurrentUser = new ChangedByEntityUser { Id = i };

            var dbContext = scope.ServiceProvider.GetRequiredService<ChangedByEntityDbContext>();

            var user = new ChangedByEntityUser
            {
                Username = $"TestUserEntity{i}"
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
            var currentUserProvider = scope.ServiceProvider.GetRequiredService<ChangedByEntityCurrentUserProvider>();
            currentUserProvider.CurrentUser = new ChangedByEntityUser { Id = i };

            var dbContext = scope.ServiceProvider.GetRequiredService<ChangedByEntityDbContext>();

            var user = new ChangedByEntityUser
            {
                Username = $"TestUserEntityAsync{i}"
            };

            dbContext.TestUsers.Add(user);
            await dbContext.SaveChangesAsync();
        }
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        var dbContext = fixture.Services.GetRequiredService<ChangedByEntityDbContext>();
        await dbContext.TestUsers.ExecuteDeleteAsync();
        await dbContext.TestUserChanges.ExecuteDeleteAsync();
    }
}