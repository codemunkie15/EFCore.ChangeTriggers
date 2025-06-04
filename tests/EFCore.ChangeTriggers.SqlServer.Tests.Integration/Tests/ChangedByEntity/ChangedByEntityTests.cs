using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByEntity.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Scopes;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByEntity;

public class ChangedByEntityTests : IClassFixture<ChangedByEntityFixture>, IDisposable
{
    private readonly ChangedByEntityFixture fixture;

    private readonly ChangedByEntityTestScope testScope;

    public ChangedByEntityTests(ChangedByEntityFixture fixture)
    {
        this.fixture = fixture;

        // Create a new service scope for each test method
        testScope = fixture.CreateTestScope();
    }

    [Fact]
    public void AddEntity_InsertsChangeEntity_WithCorrectProperties()
    {
        testScope.CurrentUserProvider.CurrentUser = ChangedByEntityUser.SystemUser;

        var user = new ChangedByEntityUser();
        testScope.DbContext.TestUsers.Add(user);
        testScope.DbContext.SaveChanges();

        var userChanges = testScope.DbContext.TestUserChanges
            .Include(uc => uc.ChangedBy)
            .Include(uc => uc.TrackedEntity)
            .Where(uc => uc.Id == user.Id).ToList();

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Insert);
        userChange.ChangedBy.Id.Should().Be(testScope.CurrentUserProvider.CurrentUser.Id);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public async Task AddEntity_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testScope.CurrentUserProvider.CurrentUserAsync = ChangedByEntityUser.SystemUser;

        var user = new ChangedByEntityUser();
        testScope.DbContext.TestUsers.Add(user);
        await testScope.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var userChanges = await testScope.DbContext.TestUserChanges
            .Include(uc => uc.ChangedBy)
            .Include(uc => uc.TrackedEntity)
            .Where(uc => uc.Id == user.Id)
            .ToListAsync(TestContext.Current.CancellationToken);

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Insert);
        userChange.ChangedBy.Id.Should().Be(testScope.CurrentUserProvider.CurrentUserAsync.Id);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    // Tests that different scopes use the correct change context
    public void AddEntity_WithMultipleScopes_InsertsChangeEntity_WithCorrectProperties()
    {
        const int numberOfScopes = 50;

        // Create users to use as change context
        testScope.CurrentUserProvider.CurrentUser = ChangedByEntityUser.SystemUser;
        var changedByUsers = Enumerable.Range(1, numberOfScopes).Select(i => new ChangedByEntityUser()).ToList();
        testScope.DbContext.TestUsers.AddRange(changedByUsers);
        testScope.DbContext.SaveChanges();

        // Dictionary to store user/changedby values to assert later
        var userLookup = new Dictionary<int, int>();

        // Create users in new scopes
        foreach (var changedByUser in changedByUsers)
        {
            using var scope = fixture.CreateTestScope();
            scope.CurrentUserProvider.CurrentUser = changedByUser;

            var testUser = new ChangedByEntityUser();
            scope.DbContext.TestUsers.Add(testUser);
            scope.DbContext.SaveChanges();

            userLookup.Add(testUser.Id, changedByUser.Id);
        }

        var testUsersFromDb = testScope.DbContext.TestUsers
            .Include(u => u.Changes)
            .Where(u => userLookup.Keys.Contains(u.Id)).ToList();

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
        testScope.CurrentUserProvider.CurrentUserAsync = ChangedByEntityUser.SystemUser;
        var changedByUsers = Enumerable.Range(1, numberOfScopes).Select(i => new ChangedByEntityUser()).ToList();
        testScope.DbContext.TestUsers.AddRange(changedByUsers);
        await testScope.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Dictionary to store user/changedby values to assert later
        var userLookup = new Dictionary<int, int>();

        // Create users in new scopes
        foreach (var changedByUser in changedByUsers)
        {
            using var scope = fixture.CreateTestScope();
            scope.CurrentUserProvider.CurrentUserAsync = changedByUser;

            var testUser = new ChangedByEntityUser();
            scope.DbContext.TestUsers.Add(testUser);
            await scope.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

            userLookup.Add(testUser.Id, changedByUser.Id);
        }

        var testUsersFromDb = await testScope.DbContext.TestUsers
            .Include(u => u.Changes)
            .Where(u => userLookup.Keys.Contains(u.Id))
            .ToListAsync(TestContext.Current.CancellationToken);

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
        testScope.CurrentUserProvider.CurrentUser = ChangedByEntityUser.SystemUser;

        var user = new ChangedByEntityUser();
        testScope.DbContext.TestUsers.Add(user);
        testScope.DbContext.SaveChanges();

        user.Username = "Modified";
        testScope.DbContext.SaveChanges();

        var userChanges = testScope.DbContext.TestUserChanges
            .Include(uc => uc.ChangedBy)
            .Include(uc => uc.TrackedEntity)
            .Where(uc => uc.Id == user.Id && uc.OperationType == OperationType.Update)
            .ToList();

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Update);
        userChange.ChangedBy.Id.Should().Be(testScope.CurrentUserProvider.CurrentUser.Id);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public async Task UpdateEntity_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testScope.CurrentUserProvider.CurrentUserAsync = ChangedByEntityUser.SystemUser;

        var user = new ChangedByEntityUser();
        testScope.DbContext.TestUsers.Add(user);
        await testScope.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        user.Username = "Modified";
        await testScope.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var userChanges = await testScope.DbContext.TestUserChanges
            .Include(uc => uc.ChangedBy)
            .Include(uc => uc.TrackedEntity)
            .Where(uc => uc.Id == user.Id && uc.OperationType == OperationType.Update)
            .ToListAsync(TestContext.Current.CancellationToken);

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Update);
        userChange.ChangedBy.Id.Should().Be(testScope.CurrentUserProvider.CurrentUserAsync.Id);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public void DeleteEntity_InsertsChangeEntity_WithCorrectProperties()
    {
        testScope.CurrentUserProvider.CurrentUser = ChangedByEntityUser.SystemUser;

        var user = new ChangedByEntityUser();
        testScope.DbContext.TestUsers.Add(user);
        testScope.DbContext.SaveChanges();

        testScope.DbContext.TestUsers.Remove(user);
        testScope.DbContext.SaveChanges();

        var userChanges = testScope.DbContext.TestUserChanges
            .Include(uc => uc.ChangedBy)
            .Include(uc => uc.TrackedEntity)
            .Where(uc => uc.Id == user.Id && uc.OperationType == OperationType.Delete)
            .ToList();

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Delete);
        userChange.ChangedBy.Id.Should().Be(testScope.CurrentUserProvider.CurrentUser.Id);
        userChange.TrackedEntity.Should().BeNull();
    }

    [Fact]
    public async Task DeleteEntity_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testScope.CurrentUserProvider.CurrentUserAsync = ChangedByEntityUser.SystemUser;

        var user = new ChangedByEntityUser();
        testScope.DbContext.TestUsers.Add(user);
        await testScope.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        testScope.DbContext.TestUsers.Remove(user);
        await testScope.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var userChanges = await testScope.DbContext.TestUserChanges
            .Include(uc => uc.ChangedBy)
            .Include(uc => uc.TrackedEntity)
            .Where(uc => uc.Id == user.Id && uc.OperationType == OperationType.Delete)
            .ToListAsync(TestContext.Current.CancellationToken);

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Delete);
        userChange.ChangedBy.Id.Should().Be(testScope.CurrentUserProvider.CurrentUserAsync.Id);
        userChange.TrackedEntity.Should().BeNull();
    }

    public void Dispose()
    {
        testScope.Dispose();
    }

    // TODO: Test when ChangedBy user has been deleted
    /*
        var userChangeEntry = TestScope.DbContext.Entry(userChange);
        userChangeEntry.Property("ChangedById").CurrentValue.Should().Be(user.Id);
    */
}