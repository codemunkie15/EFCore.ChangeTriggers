using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Domain;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Infrastructure;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity;

public class ChangeSourceEntityTests : IClassFixture<ChangeSourceEntityFixture>, IAsyncLifetime
{
    private readonly ChangeSourceEntityFixture fixture;

    public ChangeSourceEntityTests(ChangeSourceEntityFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public void MultipleScopes_SetsCorrectChangeSource()
    {
        CreateUsers();

        var dbContext = fixture.Services.GetRequiredService<ChangeSourceEntityDbContext>();
        var users = dbContext.TestUsers.Include(u => u.Changes).ThenInclude(c => c.ChangeSource).ToList();

        Assert.True(users.All(u => u.Changes.All(c => u.Username == $"ChangeSource{c.ChangeSource.Id}")));
    }

    [Fact]
    public async Task MultipleScopes_SetsCorrectChangeSource_Async()
    {
        await CreateUsersAsync();

        var dbContext = fixture.Services.GetRequiredService<ChangeSourceEntityDbContext>();
        var users = await dbContext.TestUsers.Include(u => u.Changes).ThenInclude(c => c.ChangeSource).ToListAsync();

        Assert.True(users.All(u => u.Changes.All(c => u.Username == $"ChangeSource{c.ChangeSource.Id}")));
    }

    private void CreateUsers()
    {
        for (int i = 1; i <= 10; i++)
        {
            using var scope = fixture.Services.CreateScope();
            var changeSourceProvider = scope.ServiceProvider.GetRequiredService<ChangeSourceEntityChangeSourceProvider>();
            changeSourceProvider.CurrentChangeSource = new ChangeSource { Id = i };

            var dbContext = scope.ServiceProvider.GetRequiredService<ChangeSourceEntityDbContext>();

            var user = new ChangeSourceEntityUser
            {
                Username = $"ChangeSource{i}"
            };

            dbContext.TestUsers.Add(user);
            dbContext.SaveChanges();
        }
    }

    private async Task CreateUsersAsync()
    {
        for (int i = 1; i <= 10; i++)
        {
            using var scope = fixture.Services.CreateScope();
            var changeSourceProvider = scope.ServiceProvider.GetRequiredService<ChangeSourceEntityChangeSourceProvider>();
            changeSourceProvider.CurrentChangeSource = new ChangeSource { Id = i };

            var dbContext = scope.ServiceProvider.GetRequiredService<ChangeSourceEntityDbContext>();

            var user = new ChangeSourceEntityUser
            {
                Username = $"ChangeSource{i}"
            };

            dbContext.TestUsers.Add(user);
            await dbContext.SaveChangesAsync();
        }
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        var dbContext = fixture.Services.GetRequiredService<ChangeSourceEntityDbContext>();
        await dbContext.TestUsers.ExecuteDeleteAsync();
        await dbContext.TestUserChanges.ExecuteDeleteAsync();
    }
}