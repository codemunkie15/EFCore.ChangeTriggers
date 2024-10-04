using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Domain;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Infrastructure;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar;

public class ChangeSourceScalarTests : IClassFixture<ChangeSourceScalarFixture>, IAsyncLifetime
{
    private readonly ChangeSourceScalarFixture fixture;

    public ChangeSourceScalarTests(ChangeSourceScalarFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public void MultipleScopes_SetsCorrectChangeSource()
    {
        CreateUsers();

        var dbContext = fixture.Services.GetRequiredService<ChangeSourceScalarDbContext>();
        var users = dbContext.TestUsers.Include(u => u.Changes).ToList();

        Assert.True(users.All(u => u.Changes.All(c => u.Username == $"ChangeSource{(int)c.ChangeSource}")));
    }

    [Fact]
    public async Task MultipleScopes_SetsCorrectChangeSource_Async()
    {
        await CreateUsersAsync();

        var dbContext = fixture.Services.GetRequiredService<ChangeSourceScalarDbContext>();
        var users = await dbContext.TestUsers.Include(u => u.Changes).ToListAsync();

        Assert.True(users.All(u => u.Changes.All(c => u.Username == $"ChangeSource{(int)c.ChangeSource}")));
    }

    private void CreateUsers()
    {
        for (int i = 1; i <= 10; i++)
        {
            using var scope = fixture.Services.CreateScope();
            var changeSourceProvider = scope.ServiceProvider.GetRequiredService<ChangeSourceScalarChangeSourceProvider>();
            changeSourceProvider.CurrentChangeSource = (ChangeSource)i;

            var dbContext = scope.ServiceProvider.GetRequiredService<ChangeSourceScalarDbContext>();

            var user = new ChangeSourceScalarUser
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
            var changeSourceProvider = scope.ServiceProvider.GetRequiredService<ChangeSourceScalarChangeSourceProvider>();
            changeSourceProvider.CurrentChangeSource = (ChangeSource)i;

            var dbContext = scope.ServiceProvider.GetRequiredService<ChangeSourceScalarDbContext>();

            var user = new ChangeSourceScalarUser
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
        var dbContext = fixture.Services.GetRequiredService<ChangeSourceScalarDbContext>();
        await dbContext.TestUsers.ExecuteDeleteAsync();
        await dbContext.TestUserChanges.ExecuteDeleteAsync();
    }
}