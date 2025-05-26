using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity;

public class ChangedByEntityTests : IClassFixture<ChangedByEntityFixture>
{
    private readonly ChangedByEntityFixture fixture;
    private readonly ChangedByEntityTestHelper testHelper;

    public ChangedByEntityTests(ChangedByEntityFixture fixture)
    {
        this.fixture = fixture;
        testHelper = new ChangedByEntityTestHelper(fixture.Services); // Use a new helper per test method
    }

    [Fact]
    public void AddEntity_InsertsChangeEntity_WithCorrectProperties()
    {
        testHelper.CurrentUserProvider.CurrentUser = ChangedByEntityUser.SystemUser;

        var user = testHelper.AddTestUser();
        testHelper.DbContext.SaveChanges();

        var userChanges = testHelper.GetTestUserChanges().Where(uc => uc.Id == user.Id).ToList();

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Insert);
        userChange.ChangedBy.Id.Should().Be(testHelper.CurrentUserProvider.CurrentUser.Id);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public async Task AddEntity_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testHelper.CurrentUserProvider.CurrentUserAsync = ChangedByEntityUser.SystemUser;

        var user = testHelper.AddTestUser();
        await testHelper.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var userChanges = await testHelper.GetTestUserChanges()
            .Where(uc => uc.Id == user.Id)
            .ToListAsync(TestContext.Current.CancellationToken);

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Insert);
        userChange.ChangedBy.Id.Should().Be(testHelper.CurrentUserProvider.CurrentUserAsync.Id);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    // Tests that different scopes use the correct change context
    public void AddEntity_WithMultipleScopes_InsertsChangeEntity_WithCorrectProperties()
    {
        const int numberOfScopes = 50;

        // Create users to use as change context
        testHelper.CurrentUserProvider.CurrentUser = ChangedByEntityUser.SystemUser;
        var changedByUsers = testHelper.AddTestUsers(numberOfScopes);
        testHelper.DbContext.SaveChanges();

        // Dictionary to store user/changedby values to assert later
        var userLookup = new Dictionary<int, int>();

        // Create users in new scopes
        foreach (var changedByUser in changedByUsers)
        {
            using var testHelperScoped = new ChangedByEntityTestHelper(fixture.Services);
            testHelperScoped.CurrentUserProvider.CurrentUser = changedByUser;

            var testUser = testHelperScoped.AddTestUser();
            testHelperScoped.DbContext.SaveChanges();

            userLookup.Add(testUser.Id, changedByUser.Id);
        }

        var testUsersFromDb = testHelper.GetTestUsers().Where(u => userLookup.Keys.Contains(u.Id)).ToList();

        testUsersFromDb.Should().HaveCount(numberOfScopes);
        testUsersFromDb.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangedBy.Id.Should().Be(userLookup[u.Id]);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public async Task AddEntity_WithMultipleScopes_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        const int numberOfScopes = 50;

        // Create users to use as change context
        testHelper.CurrentUserProvider.CurrentUserAsync = ChangedByEntityUser.SystemUser;
        var changedByUsers = testHelper.AddTestUsers(numberOfScopes);
        await testHelper.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Dictionary to store user/changedby values to assert later
        var userLookup = new Dictionary<int, int>();

        // Create users in new scopes
        foreach (var changedByUser in changedByUsers)
        {
            using var testHelperScoped = new ChangedByEntityTestHelper(fixture.Services);
            testHelperScoped.CurrentUserProvider.CurrentUserAsync = changedByUser;

            var testUser = testHelperScoped.AddTestUser();
            await testHelperScoped.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

            userLookup.Add(testUser.Id, changedByUser.Id);
        }

        var testUsersFromDb = await testHelper.GetTestUsers().Where(u => userLookup.Keys.Contains(u.Id)).ToListAsync(TestContext.Current.CancellationToken);

        testUsersFromDb.Should().HaveCount(numberOfScopes);
        testUsersFromDb.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangedBy.Id.Should().Be(userLookup[u.Id]);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public void UpdateEntity_InsertsChangeEntity_WithCorrectProperties()
    {
        testHelper.CurrentUserProvider.CurrentUser = ChangedByEntityUser.SystemUser;

        var user = testHelper.AddTestUser();
        testHelper.DbContext.SaveChanges();

        user.Username = "Modified";
        testHelper.DbContext.SaveChanges();

        var userChanges = testHelper.GetTestUserChanges()
            .Where(uc => uc.Id == user.Id && uc.OperationType == OperationType.Update)
            .ToList();

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Update);
        userChange.ChangedBy.Id.Should().Be(testHelper.CurrentUserProvider.CurrentUser.Id);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public async Task UpdateEntity_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testHelper.CurrentUserProvider.CurrentUserAsync = ChangedByEntityUser.SystemUser;

        var user = testHelper.AddTestUser();
        await testHelper.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        user.Username = "Modified";
        await testHelper.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var userChanges = await testHelper.GetTestUserChanges()
            .Where(uc => uc.Id == user.Id && uc.OperationType == OperationType.Update)
            .ToListAsync(TestContext.Current.CancellationToken);

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Update);
        userChange.ChangedBy.Id.Should().Be(testHelper.CurrentUserProvider.CurrentUserAsync.Id);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public void DeleteEntity_InsertsChangeEntity_WithCorrectProperties()
    {
        testHelper.CurrentUserProvider.CurrentUser = ChangedByEntityUser.SystemUser;

        var user = testHelper.AddTestUser();
        testHelper.DbContext.SaveChanges();

        testHelper.DbContext.TestUsers.Remove(user);
        testHelper.DbContext.SaveChanges();

        var userChanges = testHelper.GetTestUserChanges()
            .Where(uc => uc.Id == user.Id && uc.OperationType == OperationType.Delete)
            .ToList();

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Delete);
        userChange.ChangedBy.Id.Should().Be(testHelper.CurrentUserProvider.CurrentUser.Id);
        userChange.TrackedEntity.Should().BeNull();
    }

    [Fact]
    public async Task DeleteEntity_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testHelper.CurrentUserProvider.CurrentUserAsync = ChangedByEntityUser.SystemUser;

        var user = testHelper.AddTestUser();
        await testHelper.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        testHelper.DbContext.TestUsers.Remove(user);
        await testHelper.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var userChanges = await testHelper.GetTestUserChanges()
            .Where(uc => uc.Id == user.Id && uc.OperationType == OperationType.Delete)
            .ToListAsync(TestContext.Current.CancellationToken);

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Delete);
        userChange.ChangedBy.Id.Should().Be(testHelper.CurrentUserProvider.CurrentUserAsync.Id);
        userChange.TrackedEntity.Should().BeNull();
    }

    // TODO: Test when ChangedBy user has been deleted
    /*
        var userChangeEntry = testHelper.DbContext.Entry(userChange);
        userChangeEntry.Property("ChangedById").CurrentValue.Should().Be(user.Id);
    */
}