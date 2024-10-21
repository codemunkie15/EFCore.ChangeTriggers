using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar;

public class ChangedByScalarTests : IClassFixture<ChangedByScalarFixture>, IAsyncLifetime
{
    private readonly ChangedByScalarFixture fixture;
    private readonly ChangedByScalarTestHelper testHelper;

    public ChangedByScalarTests(ChangedByScalarFixture fixture)
    {
        this.fixture = fixture;
        testHelper = new ChangedByScalarTestHelper(fixture.Services);
    }

    [Fact]
    public void AddEntity_InsertsChangeEntity_WithCorrectProperties()
    {
        testHelper.CurrentUserProvider.CurrentUser = 1.ToString();

        var user = testHelper.AddTestUser(1);
        testHelper.DbContext.SaveChanges();

        var userChanges = testHelper.GetAllTestUserChanges().Where(uc => uc.Id == 1).ToList();

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Insert);
        userChange.ChangedBy.Should().Be(testHelper.CurrentUserProvider.CurrentUser);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public async void AddEntity_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testHelper.CurrentUserProvider.CurrentUserAsync = 1.ToString();

        var user = testHelper.AddTestUser(1);
        await testHelper.DbContext.SaveChangesAsync();

        var userChanges = await testHelper.GetAllTestUserChanges().Where(uc => uc.Id == 1).ToListAsync();

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Insert);
        userChange.ChangedBy.Should().Be(testHelper.CurrentUserProvider.CurrentUserAsync);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public void AddEntity_WithMultipleScopes_InsertsChangeEntity_WithCorrectProperties()
    {
        const int numberOfScopes = 50;

        for (int i = 1; i <= numberOfScopes; i++)
        {
            using var testHelperScoped = new ChangedByScalarTestHelper(fixture.Services);
            testHelperScoped.CurrentUserProvider.CurrentUser = i.ToString();

            testHelperScoped.AddTestUser(i);
            testHelperScoped.DbContext.SaveChanges();
        }

        var testUsers = testHelper.GetAllTestUsers().Where(u => u.Id >= 1 && u.Id <= numberOfScopes).ToList();

        testUsers.Should().HaveCount(numberOfScopes);
        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangedBy.Should().Be(u.Id.ToString());
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public async Task AddEntity_WithMultipleScopes_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        const int numberOfScopes = 50;

        for (int i = 1; i <= numberOfScopes; i++)
        {
            using var testHelperScoped = new ChangedByScalarTestHelper(fixture.Services);
            testHelperScoped.CurrentUserProvider.CurrentUserAsync = i.ToString();

            testHelperScoped.AddTestUser(i);
            await testHelperScoped.DbContext.SaveChangesAsync();
        }

        var testUsers = await testHelper.GetAllTestUsers().Where(u => u.Id >= 1 && u.Id <= numberOfScopes).ToListAsync();

        testUsers.Should().HaveCount(numberOfScopes);
        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangedBy.Should().Be(u.Id.ToString());
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public void UpdateEntity_InsertsChangeEntity_WithCorrectProperties()
    {
        testHelper.CurrentUserProvider.CurrentUser = 1.ToString();

        var user = testHelper.AddTestUser(1);
        testHelper.DbContext.SaveChanges();

        user.Username = "Modified";
        testHelper.DbContext.SaveChanges();

        var userChanges = testHelper.GetAllTestUserChanges().Where(uc => uc.Id == 1 && uc.OperationType == OperationType.Update).ToList();

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Update);
        userChange.ChangedBy.Should().Be(testHelper.CurrentUserProvider.CurrentUser);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public async void UpdateEntity_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testHelper.CurrentUserProvider.CurrentUserAsync = 1.ToString();

        var user = testHelper.AddTestUser(1);
        await testHelper.DbContext.SaveChangesAsync();

        user.Username = "Modified";
        await testHelper.DbContext.SaveChangesAsync();

        var userChanges = await testHelper.GetAllTestUserChanges().Where(uc => uc.Id == 1 && uc.OperationType == OperationType.Update).ToListAsync();

        Assert.Single(userChanges);

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Update);
        userChange.ChangedBy.Should().Be(testHelper.CurrentUserProvider.CurrentUserAsync);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public void DeleteEntity_InsertsChangeEntity_WithCorrectProperties()
    {
        testHelper.CurrentUserProvider.CurrentUser = 1.ToString();

        var user = testHelper.AddTestUser(1);
        testHelper.DbContext.SaveChanges();

        testHelper.DbContext.TestUsers.Remove(user);
        testHelper.DbContext.SaveChanges();

        var userChanges = testHelper.GetAllTestUserChanges().Where(uc => uc.Id == 1 && uc.OperationType == OperationType.Delete).ToList();

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Delete);
        userChange.ChangedBy.Should().Be(testHelper.CurrentUserProvider.CurrentUser);
        userChange.TrackedEntity.Should().BeNull();
    }

    [Fact]
    public async void DeleteEntity_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testHelper.CurrentUserProvider.CurrentUserAsync = 1.ToString();

        var user = testHelper.AddTestUser(1);
        await testHelper.DbContext.SaveChangesAsync();

        testHelper.DbContext.TestUsers.Remove(user);
        await testHelper.DbContext.SaveChangesAsync();

        var userChanges = await testHelper.GetAllTestUserChanges().Where(uc => uc.Id == 1 && uc.OperationType == OperationType.Delete).ToListAsync();

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Delete);
        userChange.ChangedBy.Should().Be(testHelper.CurrentUserProvider.CurrentUserAsync);
        userChange.TrackedEntity.Should().BeNull();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        testHelper.CurrentUserProvider.CurrentUserAsync = 1.ToString();

        // Clear database
        await testHelper.DbContext.TestUsers.ExecuteDeleteAsync();
        await testHelper.DbContext.TestUserChanges.ExecuteDeleteAsync();

        testHelper.Dispose();
    }
}