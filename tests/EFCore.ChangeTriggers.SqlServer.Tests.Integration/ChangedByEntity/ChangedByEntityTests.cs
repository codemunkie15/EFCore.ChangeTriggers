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
        const int numberOfUsers = 50;
        CreateChangedBySourceUsers(numberOfUsers);
        CreateTestUsers(numberOfUsers);

        var dbContext = fixture.Services.GetRequiredService<ChangedByEntityDbContext>();
        var testUsers = dbContext.TestUsers
            .Include(u => u.Changes)
            .ThenInclude(c => c.ChangedBy)
            .OrderBy(u => u.Id)
            .Skip(numberOfUsers)
            .ToList();

        Assert.Equal(numberOfUsers, testUsers.Count);
        Assert.True(testUsers.All(u => u.Changes.All(c => c.ChangedBy.Username == $"Change{u.Username}")));
    }

    [Fact]
    public async Task MultipleScopes_SetsCorrectChangedBy_Async()
    {
        const int numberOfUsers = 50;
        CreateChangedBySourceUsers(numberOfUsers);
        await CreateTestUsersAsync(numberOfUsers);

        var dbContext = fixture.Services.GetRequiredService<ChangedByEntityDbContext>();
        var testUsers = await dbContext.TestUsers
            .Include(u => u.Changes)
            .ThenInclude(c => c.ChangedBy)
            .OrderBy(u => u.Id)
            .Skip(numberOfUsers)
            .ToListAsync();

        Assert.Equal(numberOfUsers, testUsers.Count);
        Assert.True(testUsers.All(u => u.Changes.All(c => c.ChangedBy.Username == $"Change{u.Username}")));
    }

    private void CreateChangedBySourceUsers(int numberOfUsers)
    {
        var dbContext = fixture.Services.GetRequiredService<ChangedByEntityDbContext>();
        var currentUserProvider = fixture.Services.GetRequiredService<EntityCurrentUserProvider>();
        currentUserProvider.CurrentUser = new ChangedByEntityUser { Id = 1 };

        for (int i = 1; i <= numberOfUsers; i++)
        {
            var user = new ChangedByEntityUser { Username = $"ChangeTestUser{i}" };
            dbContext.TestUsers.Add(user);
        }

        dbContext.SaveChanges();
    }

    private void CreateTestUsers(int numberOfUsers)
    {
        for (int i = 1; i <= numberOfUsers; i++)
        {
            using var scope = fixture.Services.CreateScope();
            var currentUserProvider = scope.ServiceProvider.GetRequiredService<EntityCurrentUserProvider>();
            var dbContext = scope.ServiceProvider.GetRequiredService<ChangedByEntityDbContext>();

            currentUserProvider.CurrentUser = new ChangedByEntityUser { Id = i };

            var user = new ChangedByEntityUser
            {
                Username = $"TestUser{i}"
            };

            dbContext.TestUsers.Add(user);
            dbContext.SaveChanges();
        }
    }

    private async Task CreateTestUsersAsync(int numberOfUsers)
    {
        for (int i = 1; i <= numberOfUsers; i++)
        {
            using var scope = fixture.Services.CreateScope();
            var currentUserProvider = scope.ServiceProvider.GetRequiredService<EntityCurrentUserProvider>();
            var dbContext = scope.ServiceProvider.GetRequiredService<ChangedByEntityDbContext>();

            currentUserProvider.CurrentUser = new ChangedByEntityUser { Id = i };

            var user = new ChangedByEntityUser
            {
                Username = $"TestUser{i}"
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