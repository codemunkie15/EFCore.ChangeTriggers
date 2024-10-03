using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Domain;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Infrastructure;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar;

public class ChangedByScalarTests : IClassFixture<ChangedByScalarFixture>
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

        Assert.True(users.All(u => u.Changes.All(c => c.ChangedBy == u.Id.ToString())));

        dbContext.TestUsers.ExecuteDelete();
        dbContext.TestUserChanges.ExecuteDelete();
    }

    [Fact]
    public async Task MultipleScopes_SetsCorrectChangedBy_Async()
    {
        await CreateUsersAsync();

        var dbContext = fixture.Services.GetRequiredService<ChangedByScalarDbContext>();
        var users = await dbContext.TestUsers.Include(u => u.Changes).ToListAsync();

        Assert.True(users.All(u => u.Changes.All(c => c.ChangedBy == u.Id.ToString())));

        await dbContext.TestUsers.ExecuteDeleteAsync();
        await dbContext.TestUserChanges.ExecuteDeleteAsync();
    }

    private void CreateUsers()
    {
        for (int i = 1; i <= 100; i++)
        {
            using var scope = fixture.Services.CreateScope();
            var currentUserProvider = scope.ServiceProvider.GetRequiredService<CurrentUserProvider>();
            currentUserProvider.CurrentUser = i.ToString();

            var dbContext = scope.ServiceProvider.GetRequiredService<ChangedByScalarDbContext>();

            var user = new ScalarUser
            {
                Username = $"TestUserScalar{i}"
            };

            dbContext.TestUsers.Add(user);
            dbContext.SaveChanges();
        }
    }

    private async Task CreateUsersAsync()
    {
        for (int i = 101; i <= 200; i++)
        {
            using var scope = fixture.Services.CreateScope();
            var currentUserProvider = scope.ServiceProvider.GetRequiredService<CurrentUserProvider>();
            currentUserProvider.CurrentUser = i.ToString();

            var dbContext = scope.ServiceProvider.GetRequiredService<ChangedByScalarDbContext>();

            var user = new ScalarUser
            {
                Username = $"TestUserScalarAsync{i}"
            };

            dbContext.TestUsers.Add(user);
            await dbContext.SaveChangesAsync();
        }
    }
}